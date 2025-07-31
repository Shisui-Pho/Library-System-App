using LibrarySystem.Models;
using LibrarySystem.Models.DTO;

namespace LibrarySystem.Models.ViewModels;

public class AdminStaffDashboardViewModel
{
    public bool IsAdmin { get; set; } = false;
    public string Name { get; set; }
    public int TotalBooks { get; set; }
    public int NewBooksThisWeek { get; set; }
    public int NumberOfPendingOrders { get; set; }
    public int NumberOfOrdersThatNeedAttention { get; set; }//This are orders that need to be resolved soon
    public decimal TotalRevenue { get; set; }
    public double PercentageGrowth { get; set; }
    public int NumberOfActiveUsers { get; set; }
    public int NumberOfNewUsers { get; set; }
    public int NumberOfCancelledOrders { get; set; }
    public int NumberOfShippedOrders { get; set; }
    public int NumberOfOrdersBeingProcessed { get; set; }
    
    public int NumberOfCurrentActiveUsers { get; set; }
    public int NumberOfNewUserRegistrations { get; set; }

    //Enumeration props
    public IEnumerable<Order> RecentOrders { get; set; }
    public IEnumerable<string> TopGenres { get; set; }
    public IEnumerable<SystemAlert> SystemAlerts { get; set; }
    public IEnumerable<RecentActivities> RecentActivities { get; set; }
}
