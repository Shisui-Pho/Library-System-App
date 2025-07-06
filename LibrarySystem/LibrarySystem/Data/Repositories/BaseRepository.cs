using LibrarySystem.Data.DataAccess;
using LibrarySystem.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace LibrarySystem.Data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDBContext _dbContext;
    public BaseRepository(AppDBContext dbContext)
    {
        //Inject dbcontext
        _dbContext = dbContext;
    }
    public void Create(T entity)
    {
        //Create a new entry
        _dbContext.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public IEnumerable<T> FindAll()
    {
        return _dbContext.Set<T>();
    }

    public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        //Apply expression
        return _dbContext.Set<T>().Where(expression);
    }

    public T GetById(int id)
    {
        //Find by id
        return _dbContext.Set<T>().Find(id);
    }//GetById

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }
    public IEnumerable<T> GetWithOptions(QueryOptions<T> options)
    {
        return ApplyOptions(options);
    }//GetWithOptions
    protected IQueryable<T> ApplyOptions(QueryOptions<T> options)
    {
        IQueryable<T> q = _dbContext.Set<T>();
        //Check for the options
        if (options.HasWhere)
        {
            //Apply where clause
            q = q.Where(options.Where);
        }

        //Check for orderby clause
        if (options.HasOrderBy)
        {
            if (options.OrderByDirection == "asc")
                q = q.OrderBy(options.OrderBy);
            else
                q = q.OrderByDescending(options.OrderBy);
        }

        //Check for paging
        if (options.HasPaging)
        {
            //Select the top n items
            q = q.Skip((options.PagingInfomation.CurrentPageNumber - 1) * options.PagingInfomation.NumberOfItemsPerPage)
                 .Take(options.PagingInfomation.NumberOfItemsPerPage);
        }
        return q;
    }

    public int Count()
    { 
        return _dbContext.Set<T>().Count();
    }
}//class
