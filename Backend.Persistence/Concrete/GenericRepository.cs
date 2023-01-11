using Backend.Application.Abstract;
using Backend.Domain.EntityClass.BaseClass;
using Backend.Persistence.Context;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Persistence.Concrete
{
    public class GenericRepository<T>:IGenericRepository<T> where T : Base
    {
        private DataContext _Context;
        public GenericRepository(DataContext Context)
        {
            _Context = Context;
        }
        public async Task<bool> CreateAsync(T T)
        {
            T.CreateTime = DateTime.Now;
            var result = await _Context.Set<T>().AddAsync(T);
            return result.State == EntityState.Added;
        }
        public async Task<bool> CreateRangeAsync(List<T> liste)
        {
            await _Context.AddRangeAsync(liste);
            return true;
        }
        public IQueryable<T> GetAll(bool tracing = true)
        {
            var list = _Context.Set<T>().AsQueryable();
            if (!tracing)
            {
                list = list.AsNoTracking();
            }
            return list;
        }

        public async Task<T> GetByIdAsync(int Id, bool tracing = true)
        {
            var list = _Context.Set<T>().AsQueryable();
            if (!tracing)
            {
                list = list.AsNoTracking();
            }
            return await list.FirstOrDefaultAsync(i => i.Id == Id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool tracing = true)
        {
            var list = _Context.Set<T>().AsQueryable();
            if (!tracing)
            {
                list = list.AsNoTracking();
            }
            return await list.FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracing = true)
        {
            var list = _Context.Set<T>().AsQueryable();
            if (!tracing)
            {
                list = list.AsNoTracking();
            }
            return list.Where(expression);
        }

        public bool Remove(T T)
        {
            var result = _Context.Set<T>().Remove(T);
            return result.State == EntityState.Deleted;
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _Context.SaveChangesAsync();
            return result;
        }

        public bool Update(T T)
        {
            var model = T.ToString();
            T.OldObject += model;
            T.UpdatedTime = DateTime.Now;
            var result = _Context.Set<T>().Update(T);
            return result.State == EntityState.Modified;
        }
    }
}
