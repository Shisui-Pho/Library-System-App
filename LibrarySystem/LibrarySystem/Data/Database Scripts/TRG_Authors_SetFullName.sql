CREATE TRIGGER TRG_Authors_SetFullName
ON Author
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE A
    SET FullName = I.FirstName + ' ' + I.LastName
    FROM Author A
    INNER JOIN inserted I ON A.Id = I.Id;
END