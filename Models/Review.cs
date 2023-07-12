using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }   
        public string Text { get; set; }
        public int Rating { get; set; } 
        public Pokemon Pokemon { get; set; }
        public Reviewer Reviewer { get; set; }
    }
}
