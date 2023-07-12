using DAO.Data;
using DAO2.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAO2
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; 

        public Repository(ApplicationDbContext db) {
            _db = db;
            dbSet = db.Set<T>();
        
        }
        public void Add(T entity)
        {
            try
            { 
                if (entity is not null)
                {
                     dbSet.Add(entity);
                    
                }
            }catch(Exception ex)
            {
                Console.Write(ex.ToString());
                Console.WriteLine(ex.InnerException);
            }
            
        }

        public T Get(Expression<Func<T, bool>> filter, string? includePropreties = null)
        {
            IQueryable<T> query = dbSet.Where(filter); 
            if (includePropreties != null)
            {
                foreach (var property in includePropreties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
           
            return query.FirstOrDefault() ;
        }

        public IQueryable<T> GetAll(string? includePropreties=null)
        {
            IQueryable<T> query = dbSet;
            if(includePropreties != null)
            {
                foreach(var property in includePropreties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ) {
                query = query.Include(property);
                }
            }
            return query; 
        }
        public IQueryable<T> GetAllJoin(Expression<Func<T, bool>> filter, string? includePropreties = null)
        {
            IQueryable<T> query = dbSet.Where(filter);
            if (includePropreties != null)
            {
                foreach (var property in includePropreties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query;
        }

        public void Remove(T entity)
        {
            if(entity is not null) {
                dbSet.Remove(entity); 
               
            }
        }
        
        public void RemoveRange(IList<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
       
    }
}
