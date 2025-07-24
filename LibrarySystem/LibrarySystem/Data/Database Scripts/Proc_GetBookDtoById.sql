ALTER PROCEDURE Proc_GetBookDtoValuesByBookId
    @BookId INT
AS
BEGIN
    SELECT 
        b.Id,
        b.ISBN,
        b.BookTitle,
        b.Description,
        b.Publisher,
        b.PublicationDate,
        b.PageCount,
        b.Language,
        b.Genre,
        b.Price,
        b.AddedDate,
        AVG(CAST(bi.Rating AS FLOAT)) AS AverageRating,
        COUNT(CASE WHEN bi.Rating > 0 THEN 1 END) AS TotalRatings,
        COUNT(CASE WHEN bi.Review_ReviewText IS NOT NULL THEN 1 END) AS TotalReviews,
        COUNT(CASE WHEN bi.Viewed = 1 THEN 1 END) AS TotalViews,
        SUM(ISNULL(bi.Review_NumberOfLikes, 0)) AS TotalLikes

    FROM Book b
    LEFT JOIN BookInteraction bi ON bi.BookId = b.Id
    WHERE b.Id = @BookId
    GROUP BY 
        b.Id, b.ISBN, b.BookTitle, b.Description, b.Publisher,
        b.PublicationDate, b.PageCount, b.Language, b.Genre,
        b.Price, b.AddedDate
END
