CREATE PROCEDURE Proc_GetBooksForDtoView
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
        COUNT(CASE WHEN bi.Review IS NOT NULL THEN 1 END) AS TotalReviews,
        COUNT(CASE WHEN bi.Viewed = 1 THEN 1 END) AS TotalViews,
        SUM(ISNULL(bi.Review.NumberOfLikes, 0)) AS TotalLikes,
        STRING_AGG(a.Name, ', ') AS Authors
    FROM Book b
    LEFT JOIN BookInteraction bi ON bi.BookId = b.Id
    LEFT JOIN BookAuthor ba ON ba.BookId = b.Id
    LEFT JOIN Author a ON a.Id = ba.AuthorId
    GROUP BY 
        b.Id, b.ISBN, b.BookTitle, b.Description, b.Publisher,
        b.PublicationDate, b.PageCount, b.Language, b.Genre,
        b.Price, b.AddedDate
END
