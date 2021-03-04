using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesAPI_GC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoviesAPI_GC.Controllers
{
    public class HomeController : Controller
    {
        private MovieDAL md = new MovieDAL();

        private readonly MovieAPIDbContext _favoriteDB;

        public HomeController(MovieAPIDbContext favoritesContext)
        {
            _favoriteDB = favoritesContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewSingleMovie(int id)
        {
            Movie a = md.singleMovie(id);
            return View(a);
        }

        public IActionResult Search(string search)
        {
            List<Movie> movies = md.SearchMovies(search);
            return View(movies);
        }

        public IActionResult Favorites()
        {
            // finds users favorite movies and returns list to view

            // first access database
            string user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<FavoriteMovies> userFavorites = _favoriteDB.FavoriteMovies.Where(x => x.UserId == user).ToList();

            // consult API
            List<Movie> movies = new List<Movie>();
            foreach (FavoriteMovies fm in userFavorites)
            {
                movies.Add(md.singleMovie(fm.FavId));
            }
            return View(movies);
        }

        public IActionResult Delete(int id)
        {
            string user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<FavoriteMovies> userFavorites = _favoriteDB.FavoriteMovies.Where(x => x.UserId == user).ToList();

            // id represents the movie id to be removed
            FavoriteMovies deleteMovie = userFavorites.Find(x => x.Id == id);

            _favoriteDB.Remove(deleteMovie.Id);                                                                                                                                                                                                                                                     
            _favoriteDB.SaveChanges();
                                                                                                                            
            return RedirectToAction("Favorites");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
