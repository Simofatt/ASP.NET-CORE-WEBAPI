using DAO2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAO.Data;
using Microsoft.Identity.Client;

namespace DAO2
{
    public class ReviewsRepository : Repository<Review>, IReviewsRepository
    {
        private readonly ApplicationDbContext _db;

        public ReviewsRepository(ApplicationDbContext db) :base(db)
        
        {
            _db = db;
        }
        public void Update(Review review)
        {
          
            Review reviewDb = _db.Reviews.Where(r=>r.Id == review.Id).FirstOrDefault();
            if (reviewDb is not null)
            {
                if (review.Title is  not null)
                      reviewDb.Title = review.Title;
                if(review.Text is not null)
                     reviewDb.Text = review.Text;
                if(review.Rating !=0)
                    review.Rating = review.Rating;
               
            }
           
           

        }
        public bool ItExists (int Id)
        {
           bool result  =_db.Reviews.Any(r=>r.Id == Id);
             return result;
        }

        public IQueryable<Review> GetReviewByPokemonId(int pokemonId)
        {
            var reviews = from p in _db.Pokemons
                          where p.Id == pokemonId
                          join r in _db.Reviews
                          on p.Id equals r.Pokemon.Id
                          select r; 
            return reviews;
                
                
                }
    }
}
