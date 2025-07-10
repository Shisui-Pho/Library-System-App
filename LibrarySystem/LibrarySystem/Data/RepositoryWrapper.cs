using LibrarySystem.Data.Repositories;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;

namespace LibrarySystem.Data;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly AppDBContext _dbContext;
    private IBookRepository bookRepository;
    private IAuthorRepository authorRepository;
    private IMessageReuqestsRepository messageReuqestsRepository;
    private IOrderRepository orderRepository;
    private ICartRepository cartRepository;
    private IBaseRepository<PickupPoint> pickupPointRepository;
    private IBaseRepository<PaymentMethod> paymentMethodsRepository;


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

    public IOrderRepository Orders
    {
        get
        {
            orderRepository ??= new OrderRepository(_dbContext);
            return orderRepository;
        }
    }

    public ICartRepository Carts
    {
        get
        {
            cartRepository ??= new CartRepository(_dbContext);
            return cartRepository;
        }
    }

    public IBaseRepository<PickupPoint> PickupPoints
    {
        get
        {
            pickupPointRepository ??= new BaseRepository<PickupPoint>(_dbContext);
            return pickupPointRepository;
        }
    }

    public IBaseRepository<PaymentMethod> PaymentMethods
    {
        get
        {
            paymentMethodsRepository ??= new BaseRepository<PaymentMethod>(_dbContext);
            return paymentMethodsRepository;
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