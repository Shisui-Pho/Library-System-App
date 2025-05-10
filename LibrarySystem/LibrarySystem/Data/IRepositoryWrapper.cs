namespace LibrarySystem.Data;

public interface IRepositoryWrapper
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    void SaveChanges();
}
