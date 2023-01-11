using System.Linq.Expressions;

namespace Backend.Application.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        /////////////////////////////////////////////////////////////////////////////////////////

        //select multitude select process
        IQueryable<T> GetAll(bool tracing = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracing = true);
        //select one 
        Task<T> GetByIdAsync(int Id, bool tracing = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool tracing = true);
        ///////////////////////////////////////////////////////////////////////////////////////

        // write process
        Task<bool> CreateAsync(T T);
        Task<bool> CreateRangeAsync(List<T> liste);
        bool Update(T T);
        bool Remove(T T);
        //////////////////////////////////////////////////////////////////////////////////////

        // Update database 
        Task<int> SaveChangesAsync();
    }
}
