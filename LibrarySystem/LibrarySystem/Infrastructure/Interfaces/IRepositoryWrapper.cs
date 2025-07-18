﻿using LibrarySystem.Models;

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
    public IBaseRepository<DeliveryAddress> DeliveryAddresses { get; }
    void SaveChanges();
}
