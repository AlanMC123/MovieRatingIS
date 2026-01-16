using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRatingIS.Model
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public decimal Rating { get; set; }
    }
}