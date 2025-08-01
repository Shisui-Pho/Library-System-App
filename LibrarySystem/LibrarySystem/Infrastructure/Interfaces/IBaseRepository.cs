using LibrarySystem.Data.DataAccess;
using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using System.Linq.Expressions;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IBaseRepository<T> where T : class
{
    int Count();
    int Count(Expression<Func<T, bool>> expression);
    T GetById(int id);
    IEnumerable<T> FindAll();
    IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
    IEnumerable<T> GetWithOptions(QueryOptions<T> options);

    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    IEnumerable<TOutput> GetWithOptions<TOutput>(AdvancedQueryOptions<T, TOutput> options);
}
