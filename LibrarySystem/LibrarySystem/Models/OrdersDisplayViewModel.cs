using LibrarySystem.Models.ViewModels;

namespace LibrarySystem.Models;
public class OrdersDisplayViewModel
{
    public IEnumerable<OrderViewModel> Orders { get; set; }
    public PagingInfomation PagingInfomation { get; set; }
}
