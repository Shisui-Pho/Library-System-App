using LibrarySystem.Infrastructure.Enums;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Common;

namespace LibrarySystem.Infrastructure;
public class CheckoutService : ICheckoutService
{
    private readonly IRepositoryWrapper _repo;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;

    public CheckoutService(IRepositoryWrapper repo, IUserService userService, ICartService cartService)
    {
        this._repo = repo;
        this._userService = userService;
        this._cartService = cartService;
    }
    public async Task<CheckoutViewModel> PrepareForCheckoutAsync(HttpContext context)
    {
        var cart = _cartService.GetCart(context);
        
        if (cart == null || !cart.CartItems.Any())
        {
            return null;
        }

        var model = new CheckoutViewModel { Cart = cart };
        // Populate the model with user information and other necessary data
        await PopulateCheckoutViewModel(model, context);

        //I will implement saving the user info to the session variable
        // later on when I implement multiple pages for checkout
        //- for now, we will just return the model with the cart and user info
        return model;
    }
    public async Task<(bool success, int orderID)> CheckoutAsync(HttpContext context, CheckoutViewModel model)
    {
        //Here I need to log this to the database
        var order = CreateOrder(model, context);
        if(order == null)
        {
            //Repopulate model
            await PopulateCheckoutViewModel(model, context);
            return (false, 0); // If we couldn't create the order, return false
        }

        //Here we will save the order to the database
        try
        {
            _repo.Orders.Create(order);
            _repo.SaveChanges();
            return (true, order.OrderId); //Indicate success if the order was created and saved successfully
        }
        catch (DbException)
        {
            //For now we will just catch the exception and return false
            await PopulateCheckoutViewModel(model, context);
            return (false, 0); //Indicate failure if an exception occurs
        }

    }
    private Order CreateOrder(CheckoutViewModel model, HttpContext context)
    {
        //This method should create a new order based on the model
        var orderItems = model.Cart.CartItems.Select(ci => new OrderItem
        {
            BookId = ci.BookID,
            Quantity = ci.Quantity,
            UnitPrice = ci.Price
        });

        var successful = TryGetDeliveryAddressId(model, context, out int? addressId);
        if(!successful && addressId == 0) // 0 indicates failure to get the delivery addressId
        {
            return null; // If we couldn't get the delivery addressId return null
        }

        //var suc = Enum.TryParse<DeliveryOption>(model.DeliveryOption, out var deliveryOption);
        var order = new Order
        {
            BookOrderItems = [.. orderItems],
            UserID = _userService.GetUserId(context.User),
            Description = "-",
            TotalPrice = model.Cart.CalculateTotalPrice(),
            Status = BookOrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            PaymentMethodId = model.PaymentMethodId,
            DeliveryOption = model.DeliveryOption == "Delivery" ? DeliveryOption.Delivery : DeliveryOption.Pickup,
            DeliveryAddressId = addressId,
            PickupPointId = model.DeliveryOption == "Pickup" ? model.PickupPointId : null
        };
        return order;
    }
    private bool TryGetDeliveryAddressId(CheckoutViewModel model, HttpContext context, out int? deliveryAddressId)
    {
        deliveryAddressId = null;
        // This method should return the delivery addressId ID based on the model
        if (model.DeliveryOption != "Delivery")
            return false; // No delivery addressId needed for pickup

        var userId = _userService.GetUserId(context.User);

        //First check if the delivery addressId already exists
        var address = _repo.DeliveryAddresses.FindByCondition(da => da.UserId == userId).FirstOrDefault();
        try
        {
            if (address != null)
            {
                //I need to update the existing addressId instead of creating a new one
                address.AddressLine1 = model.AddressLine1;
                address.AddressLine2 = model.AddressLine2;
                address.City = model.City;
                address.PostalCode = model.PostalCode;
                address.Province = model.Province;
                _repo.DeliveryAddresses.Update(address);
            }
            else
            {
                //We need to create a new delivery addressId
                var newAddress = new DeliveryAddress
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    Province = model.Province,
                    UserId = userId
                };
                _repo.DeliveryAddresses.Create(newAddress);
            }
            _repo.SaveChanges();
            //The ID field will be automatically set by the database after saving the changes
            deliveryAddressId = address.Id;
            return true;
        }
        catch(DbException)
        {
            //for now we will just catch the exception and return false
            deliveryAddressId = 0; // Set to 0 to indicate failure
            return false; // Indicate failure if an exception occurs
        }
    }//TryGetDeliveryAddressId
    public async Task PopulateCheckoutViewModel(CheckoutViewModel model, HttpContext context)
    {
        var user = await _userService.GetCurrentLoggedInUserAsync(context.User);
        model.Email = user.Email;
        model.FirstName = user.FirstName;
        model.LastName = user.LastName;
        PopulatePaymentMethods(model);
        PopulateProvinces(model);
    }
    private void PopulatePaymentMethods(CheckoutViewModel model)
    {
        // This method should populate the payment methods for the view
        // For example, you can fetch available payment methods from the database
        // and assign them to the model's PaymentMethods property.
        model.PaymentMethods = _repo.PaymentMethods.FindByCondition(pm => pm.IsActive);
    }
    private void PopulateProvinces(CheckoutViewModel model)
    {
        // This method should populate the delivery options for the view
        var pp = _repo.PickupPoints.FindAll();
        model.Provinces = _repo.PickupPoints.FindByCondition(pp => pp.IsActive)
                               .Select(x => (x.Province, x.ProvinceCode))
                               .Distinct()
                               .Select(pp => new SelectListItem(pp.Province, pp.ProvinceCode));
    }
}//CheckoutService
