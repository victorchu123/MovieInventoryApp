using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieInventoryApp.Models
{
    public class Date
    {
        public int Month;
        public int Day;
        public int Year;
    }

    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Date ReleaseDate { get; set; }
        public string Genre { get; set; }
        public Rank IMDBRank { get; set; }
    }

    public class Rank
    {
        public decimal Rating { get; set; }
        public long Votes { get; set; }

    }

    public class MovieDBContext: DbContext
    {
        public DbSet<Movie> Movies { get; set; }
    }
}