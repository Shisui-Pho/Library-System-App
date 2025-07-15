using LibrarySystem.Models.ViewModels;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface ICheckoutService
{
    Task<CheckoutViewModel> PrepareForCheckoutAsync(HttpContext context);
    Task<(bool success, int orderID)> CheckoutAsync(HttpContext context, CheckoutViewModel model);
    Task PopulateCheckoutViewModel(CheckoutViewModel model, HttpContext context);
}//ICheckoutService
