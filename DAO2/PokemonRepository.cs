using DAO.Data;
using DAO2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAO2
{
    public class PokemonRepository : Repository<Pokemon> , IPokemonRepository 
    {
        private readonly ApplicationDbContext _db;
        public PokemonRepository(ApplicationDbContext db) : base(db) {
            _db = db; 
        
        }

        public void Update(Pokemon pokemon)
        {
            if (pokemon is not null)
            {
                Pokemon objDb = _db.Pokemons.Where(p => p.Id == pokemon.Id).FirstOrDefault();
                if(objDb != null)
                {
                    if(pokemon.Name is not null )
                    {
                        objDb.Name = pokemon.Name;
                    }
                    if(pokemon.BirthDate is not null  )
                    {
                        objDb.BirthDate = pokemon.BirthDate;
                    }
                }
            }
        }


        public bool EntityExists(int Id)
        {

           return _db.Pokemons.Any(e => e.Id == Id);
 
        }

        public void AddPokemonData(int OwnerId,int CategoryId,Pokemon pokemon)
        {
            _db.Pokemons.Add(pokemon);


            var Owner = _db.Owners.Where(o=>o.Id ==OwnerId).FirstOrDefault();
            PokemonOwner pokemonOwner = new()
            {
                Owner = Owner,
                Pokemon = pokemon,
            };
            _db.PokemonOwners.Add(pokemonOwner);

            var Category = _db.Categories.Where(c=>c.Id == CategoryId).FirstOrDefault();    
           PokemonCategory pokemonCategory = new()
           {
               Category= Category,
               Pokemon = pokemon
           };
            
            _db.PokemonCategories.Add(pokemonCategory);

        }
    }
}
