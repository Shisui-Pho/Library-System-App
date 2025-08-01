CREATE PROCEDURE Proc_GetTopGenres
			@TopN INT = 5
AS
BEGIN
	SELECT TOP (@TopN) Genre.Name, Count(*) FROM OrderItem
	JOIN Book
	ON Book.Id = OrderItem.BookId
	JOIN BookGenre 
	ON BookGenre.BooksId = Book.Id
	JOIN Genre
	ON Genre.Id = BookGenre.GenresId
	GROUP BY Genre.Name
	ORDER BY Count(*) DESC

	-- In the future I may need to add a Union for a second query to this one which will be 
	  -- based on the wishlist as well as cartlist
END



