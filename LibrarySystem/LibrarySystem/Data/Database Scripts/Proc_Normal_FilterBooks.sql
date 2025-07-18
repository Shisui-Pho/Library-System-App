ALTER PROCEDURE Proc_Normal_FilterBooks
    @Top NVARCHAR(50) = NULL,
    @Genre NVARCHAR(100) = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @PriceRange NVARCHAR(50) = NULL,
    @Page INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    CREATE TABLE #BookWithRowNum (
        Id INT,
        ISBN NVARCHAR(50),
        BookTitle NVARCHAR(255),
        [Description] NVARCHAR(MAX),
        Publisher NVARCHAR(255),
        PublicationDate DATETIME,
        [PageCount] INT,
        [Language] NVARCHAR(100),
        Genre NVARCHAR(100),
        Price DECIMAL(18, 2),
        AddedDate DATE,
        RowNum INT
    );

    INSERT INTO #BookWithRowNum
    SELECT 
        b.Id, b.ISBN, b.BookTitle, b.Description, b.Publisher,
        b.PublicationDate, b.PageCount, b.Language, b.Genre,
        b.Price, b.AddedDate,
        ROW_NUMBER() OVER (
            ORDER BY
                CASE 
                    WHEN @Top = 'popular' THEN 
                        (SELECT COUNT(*) FROM BookInteraction bi WHERE bi.BookId = b.Id AND bi.Viewed = 1)
                    WHEN @Top = 'top-rated' THEN 
                        (SELECT AVG(CAST(bi.Rating AS FLOAT)) FROM BookInteraction bi WHERE bi.BookId = b.Id)
                    WHEN @Top = 'new-release' THEN 
                        CONVERT(FLOAT, DATEDIFF(DAY, '2000-01-01', b.PublicationDate))
                    WHEN @Top = 'hidden-germs' THEN 
                        (SELECT AVG(CAST(bi.Rating AS FLOAT)) FROM BookInteraction bi WHERE bi.BookId = b.Id)
                    WHEN @Top = 'staff-picks' THEN 
                        (SELECT SUM(ISNULL(bi.Review_NumberOfLikes, 0)) FROM BookInteraction bi WHERE bi.BookId = b.Id)
                    ELSE NULL
                END DESC
        )
    FROM Book b
    WHERE 
        (@Genre IS NULL OR b.Genre = @Genre)
        AND (
            @SearchTerm IS NULL OR 
            b.BookTitle LIKE '%' + @SearchTerm + '%' OR 
            b.Description LIKE '%' + @SearchTerm + '%' OR 
            b.ISBN LIKE '%' + @SearchTerm + '%'
        )
        AND (
            @PriceRange IS NULL OR
            (
                (@PriceRange = 'under-200' AND b.Price < 200) OR
                (@PriceRange = '200-500' AND b.Price BETWEEN 200 AND 500) OR
                (@PriceRange = '500-1000' AND b.Price BETWEEN 500 AND 1000) OR
                (@PriceRange = '100-plus' AND b.Price > 100)
            )
        )
        AND (
            @Top IS NULL OR
            (
                @Top = 'popular' AND EXISTS (
                    SELECT 1 FROM BookInteraction bi WHERE bi.BookId = b.Id AND bi.Viewed = 1
                )
                OR @Top = 'top-rated' AND EXISTS (
                    SELECT 1 FROM BookInteraction bi WHERE bi.BookId = b.Id AND bi.Rating IS NOT NULL
                )
                OR @Top = 'new-release'
                OR @Top = 'hidden-germs' AND (
                    (SELECT COUNT(*) FROM BookInteraction bi WHERE bi.BookId = b.Id AND bi.Viewed = 1) < 5 AND
                    (SELECT AVG(CAST(bi.Rating AS FLOAT)) FROM BookInteraction bi WHERE bi.BookId = b.Id) >= 4.0
                )
                OR @Top = 'staff-picks' AND EXISTS (
                    SELECT 1 FROM BookInteraction bi 
                    WHERE bi.BookId = b.Id AND ISNULL(bi.Review_NumberOfLikes, 0) >= 5
                )
            )
        );

    -- Get total count
    SELECT @TotalCount = COUNT(*) FROM #BookWithRowNum;

    -- Return paginated rows
    SELECT *
    FROM #BookWithRowNum
    WHERE RowNum BETWEEN @Offset + 1 AND @Offset + @PageSize;

    DROP TABLE #BookWithRowNum;
END