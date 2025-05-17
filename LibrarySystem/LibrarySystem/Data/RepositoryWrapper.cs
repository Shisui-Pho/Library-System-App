namespace LibrarySystem.Data;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly AppDBContext _dbContext;
    private IBookRepository bookRepository;
    private IAuthorRepository authorRepository;
    private IMessageReuqestsRepository messageReuqestsRepository;

    public IBookRepository Books
    {
        get
        {
            bookRepository ??= new BookRepository(_dbContext);
            return bookRepository;
        }
    }

    public IAuthorRepository Authors
    {
        get
        {
            authorRepository ??= new AuthorRepository(_dbContext);
            return authorRepository;
        }
    }

    public IMessageReuqestsRepository MessageReuqests
    {
        get
        {
            messageReuqestsRepository ??= new MessageRequestRepository(_dbContext);
            return messageReuqestsRepository;
        }
    }

    public RepositoryWrapper(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}