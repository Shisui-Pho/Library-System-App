using LibrarySystem.Data.DataAccess;
using System.Linq.Expressions;

namespace LibrarySystem.Infrastructure.Interfaces;
public interface IBaseRepository<T> where T : class
{
    int Count();
    T GetById(int id);
    IEnumerable<T> FindAll();
    IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
    IEnumerable<T> GetWithOptions(QueryOptions<T> options);

    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}
