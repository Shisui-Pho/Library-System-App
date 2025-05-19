using LibrarySystem.Models.ViewModels;
using System.Linq.Expressions;

namespace LibrarySystem.Data.DataAccess;

public class QueryOptions<T> where T : class
{
    public Expression<Func<T, Object>> OrderBy { get; set; }
    public string OrderByDirection { get; set; } = "asc"; //default
    public Expression<Func<T, bool>> Where { get; set; }
    public PagingInfomation PagingInfomation { get; set; }


    public bool HasWhere => Where != null;
    public bool HasOrderBy => OrderBy != null;
    public bool HasPaging => PagingInfomation != null;
}//class