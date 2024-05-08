using IMS.Infrastructure.IRepository;
using IMS.Modes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Repository.CRUD
{
    public class CrudService<TEntity> : ICrudService<TEntity> where TEntity : BaseEntity
    {
        private readonly IMSDbContext _context;
        public CrudService(IMSDbContext context)
        {
            _context=context;

        }

        public int Delete(TEntity entity)
        {
            var result = _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            var result = _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public TEntity Get(int? id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression).SingleOrDefault();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return  await _context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncIncludingProperties(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);

            foreach (var include in includes)
            {
                query = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Include(query, include);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(int? id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).SingleOrDefaultAsync();
        }

        public int Insert(TEntity entity)
        {
            var result = _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public async Task<int> InsertAsync(TEntity entity)
        {
            var result = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<TEntity> QueryAsync(string commandText, object param = null, CommandType commandType = CommandType.Text)
        {
            return await _context.Set<TEntity>().FromSqlRaw(commandText,param).FirstOrDefaultAsync(); 
            
        }

        public async Task<IEnumerable<object>> QueryListAsync(string commandText, object param = null, CommandType commandType = CommandType.Text)
        {
            var result = await _context.Set<TEntity>().FromSqlRaw(commandText, param).ToListAsync();
            var result_list = new List<object>();
            foreach(var item in result)
            {
                result.Add(item);
            }
            return result_list;
        }

        public int Update(TEntity entity)
        {
            var result = _context.Set<TEntity>().Update(entity).Property(p=>p.Id);
            _context.SaveChanges();
            return entity.Id;
        }
        
        public async Task<int> UpdateAsync(TEntity entity)
        {
            var result = _context.Set<TEntity>().Update(entity).Property(p => p.Id);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        

    }

}
