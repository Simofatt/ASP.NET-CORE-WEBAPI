using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO2.Interfaces
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        void Update (Owner owner);
        IQueryable<Pokemon> GetPokemonByOwner(int  ownerId);
        IQueryable<Owner> GetOwnerOfAPokemon(int pokemonId);
        bool EntityExists(int Id);
    }
}
