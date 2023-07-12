using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Pokemon
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public DateTime?  BirthDate { get; set; }    
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }

    }
}
