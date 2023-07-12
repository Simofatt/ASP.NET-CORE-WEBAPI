using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO2.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
        bool ItExists(int id);
        public IQueryable<Pokemon> GetPokemonByCategory(int Id);
    }
}
