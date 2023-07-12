using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public  class Country
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public ICollection<Owner> Owners  { get; set; }
    }
}
