using LibrarySystem.Data.DataAccess;
using System.Linq.Expressions;

namespace LibrarySystem.Data.Repositories;
public interface IBaseRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> FindAll();
    IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
    IEnumerable<T> GetWithOptions(QueryOptions<T> options);

    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
