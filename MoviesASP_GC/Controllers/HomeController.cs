using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesAPI_GC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI_GC.Controllers
{
    public class HomeController : Controller
    {
        private MovieDAL md = new MovieDAL();

        public IActionResult Index(string sortMethod, int pageNo)
        {
            pageNo = 1;
            List<Result> featured = md.SortFeatured(sortMethod, pageNo);
            return View(featured);
        }




        public IActionResult Search(string search)
        {
            List<Movie> movies = md.SearchMovies(search);
            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Results(string search)
        {
            List<Movie> searchResults = md.SearchMovies(search);
            return View(searchResults);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
