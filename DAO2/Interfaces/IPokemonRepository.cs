using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace DAO2.Interfaces
{
    public interface IPokemonRepository :IRepository<Pokemon>
    {
        void Update(Pokemon pokemon);
        bool EntityExists(int Id);
        void AddPokemonData(int OwnerId,int categoryId,Pokemon pokemon);   
    }
}
