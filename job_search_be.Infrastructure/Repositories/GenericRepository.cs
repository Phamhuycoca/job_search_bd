using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace job_search_be.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {

        private readonly job_search_DbContext _context;
        DbSet<T> _dbSet;
        public GenericRepository(job_search_DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public List<T> GetAllData()
        {
            return _dbSet.ToList();
        }
        public void Create(T entity)
        {
            try
            {
                if (!_dbSet.Any(e => e == entity))
                {
                    _dbSet.Add(entity);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error in Create method: {ex.Message}");
                throw;
            }
        }

        public void Delete(long id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error in Delete method: {ex.Message}");
                throw;
            }
        }


        public T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            if (!_dbSet.Any(e => e == entity))
            {
                throw new Exception("Not found");
            }
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error in Delete method: {ex.Message}");
                throw;
            }
        }
    }
}
