using System;
using System.Collections.Generic;

namespace MoviesAPI_GC.Models
{
    public partial class FavoriteMovies
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FavId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
