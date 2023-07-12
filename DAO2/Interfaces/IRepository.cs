using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAO2.Interfaces
{
    public interface IRepository <T> where T : class
    {
        IQueryable<T> GetAll(string? includePropreties=null);   
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IList<T> entity);
        T Get(Expression<Func<T,bool>> filter , string? includePropreties=null);
        IQueryable<T> GetAllJoin(Expression<Func<T, bool>> filter, string? includePropreties = null);
    }
}
