using Microsoft.AspNetCore.Authorization;
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

        public IActionResult ViewSingleMovie(int id)
        {
            Movie a = new Movie();
            try
            {
                a = md.singleMovie(id);
                return View(a);
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Mistake");

        }

        public IActionResult Mistake()
        {
            return View();
        }

            public IActionResult Index(string sortMethod, int   pageNo)
        {
            pageNo = 1;
            List<Movie> featured = md.SortFeatured(sortMethod, pageNo);
            return View(featured);
        }

        public IActionResult Search(string search, int? pageNo)
        {
            int page = pageNo ?? 1;
            TempData["page"] = page;
            TempData["CurrentSearch"] = search;
            Rootobject query = md.SearchMovies(search,page);
            TempData["totalPages"] = query.total_pages;
            List<Movie> movies = query.results.ToList();
            return View(movies);
        }
        [Authorize]
        public IActionResult AddToFavorites(int id)
        {
            string user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<FavoriteMovies> userFavorites = _favoriteDB.FavoriteMovies.Where(x => x.UserId == user).ToList();
            if (userFavorites.Where(x => x.FavId == id).Count() > 0)
            {
                TempData["error"] = "This Movie is already in your list of favorites";
                return RedirectToAction("ViewSingleMovie", new { id = id });
               
            }
            else
            {
                FavoriteMovies newFav = new FavoriteMovies();
                newFav.UserId = user;
                newFav.FavId = id;
                _favoriteDB.FavoriteMovies.Add(newFav);
                _favoriteDB.SaveChanges();
                return RedirectToAction("Favorites");
            }

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
            FavoriteMovies deleteMovie = userFavorites.FirstOrDefault(x => x.FavId == id);

            _favoriteDB.FavoriteMovies.Remove(deleteMovie);
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
