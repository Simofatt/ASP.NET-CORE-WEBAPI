using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO2.Interfaces
{
    public interface IReviewsRepository: IRepository<Review>
    {

        void Update(Review review);
        bool ItExists(int Id);
        IQueryable<Review> GetReviewByPokemonId(int pokemonId);
    }
}
