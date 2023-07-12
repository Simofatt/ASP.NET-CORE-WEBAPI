using DAO.Data;
using DAO2.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO2
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {

        _db = db;   
        }

        public bool ItExists(int id)
        {
            bool itExits = _db.Categories.Any(c=> c.Id == id);  

            return itExits;
        }

        public void Update(Category category)
        {
            var categoryDb = _db.Categories.Where(c=>category.Id == category.Id).FirstOrDefault();

            if (categoryDb is not null)
            {
                if (category.Name is not null)
                {
                    categoryDb.Name = category.Name;
                }
            }
        }

        public IQueryable<Pokemon> GetPokemonByCategory(int Id)
        {
            IQueryable<Pokemon> result = from pc in _db.PokemonCategories

                         join pokemons in _db.Pokemons
                         on pc.PokemonId equals pokemons.Id
                         where pc.CategoryId == Id    
                         select new  Pokemon
                         {
                             Id = pokemons.Id,
                             Name = pokemons.Name,
                             BirthDate = pokemons.BirthDate
                             
                         };
            return result; 
        }
    }

}