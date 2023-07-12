using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Data;
using DAO2.Interfaces;
using Models;

namespace DAO2
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {

        private readonly ApplicationDbContext _db;
        public OwnerRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }    

        public IQueryable<Owner> GetOwnerOfAPokemon(int pokemonId)
        {
            IQueryable<Owner> owner = from po in _db.PokemonOwners
                                      where po.PokemonId == pokemonId
                                      join o in _db.Owners
                                      on po.OwnerId equals o.Id
                                      select o;
            return owner;
        }

        public IQueryable<Pokemon> GetPokemonByOwner(int ownerId)
        {
            IQueryable<Pokemon> pokemon = from po in _db.PokemonOwners
                                         where po.OwnerId == ownerId
                                         join p in _db.Pokemons
                                         on po.PokemonId equals p.Id
                                         select p;
            return pokemon;
        }

        public void Update(Owner owner)
        {
           var result = _db.Owners.Where(o=>o.Id == owner.Id).FirstOrDefault();
            if (result is not null)
            {
                if (owner.FirstName is not null)
                    result.FirstName = owner.FirstName;
                if (owner.LastName is not null)
                    result.LastName = owner.LastName;
                if (owner.Gym is not null)
                    result.Gym = owner.Gym;
            }
        }

        public bool EntityExists(int Id)
        {

            return _db.Pokemons.Any(e => e.Id == Id);

        }
    }
}
