using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO2.Interfaces
{
    public interface IUnitOfWork
    {
        public IPokemonRepository Pokemon{ get; set; }
        public ICategoryRepository Category { get; set; }
        public ICountryRepository Country { get; set; } 
        public IOwnerRepository Owner { get; set; }
        public IReviewsRepository Reviews { get; set; }
        public bool Save();
    }
}
