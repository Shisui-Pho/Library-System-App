using LibrarySystem.Models;

namespace LibrarySystem.Infrastructure.Interfaces;

public interface IRepositoryWrapper
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    IMessageReuqestsRepository MessageReuqests { get; }
    IOrderRepository Orders { get; }
    ICartRepository Carts { get; }
    IBaseRepository<PickupPoint> PickupPoints { get; }
    IBaseRepository<PaymentMethod> PaymentMethods { get; }  
    IBaseRepository<Genre> Genres { get; }
    IBaseRepository<DeliveryAddress> DeliveryAddresses { get; }
    IBaseRepository<ReviewInteraction> ReviewInteractions { get; }
    IBaseRepository<BookInteraction> BookInteractions {  get; }
    void SaveChanges();
}
