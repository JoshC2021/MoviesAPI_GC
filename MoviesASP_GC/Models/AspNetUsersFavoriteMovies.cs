using System;
using System.Collections.Generic;

namespace MoviesAPI_GC.Models
{
    public partial class AspNetUsersFavoriteMovies
    {
        public string UserId { get; set; }
        public int FavId { get; set; }

        public virtual FavoriteMovies Fav { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
