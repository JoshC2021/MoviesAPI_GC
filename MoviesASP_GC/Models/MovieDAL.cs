using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MoviesAPI_GC.Models
{
    public class MovieDAL
    {
        public string GetData(string searchName)
        {
            // something off here

            string url = $"https://api.themoviedb.org/3/search/movie?api_key={Secret.MovieAPIKey}&query={searchName}"; // Movie Model

            //Web Requests sometimes need Headers/User Agent prop
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = null;
            response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string json = rd.ReadToEnd();
            return json;
        }

        public string GetData(int iD)
        {
            // something off here

            string url = $"https://api.themoviedb.org/3/movie/{iD}?api_key={Secret.MovieAPIKey}"; // Search Model

            //Web Requests sometimes need Headers/User Agent prop
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = null;

            response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string json = rd.ReadToEnd();
            return json;
        }
        public string GetData(string sortMethod, int pageNo) // Featured-index
        {
            string url = $"https://api.themoviedb.org/3/discover/movie?api_key={Secret.MovieAPIKey}&language=en-US&sort_by={sortMethod}&include_adult=false&include_video=false&page={pageNo}"; 

            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = null;
            response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string json = rd.ReadToEnd();
            return json;
        }

        public Movie singleMovie(int iD)
        {
            string json = GetData(iD);
            Movie r = JsonConvert.DeserializeObject<Movie>(json); 
            return r;
        }

        public List<Movie> SearchMovies(string searchName)
        {
            string json = GetData(searchName);
            Rootobject r = JsonConvert.DeserializeObject<Rootobject>(json);
            List<Movie> qMovies = r.results.ToList();
            return qMovies;

        }
        public List<Movie> SortFeatured(string sortMethod, int pageNo)
        {
            string json = GetData(sortMethod, pageNo);
            Rootobject f = JsonConvert.DeserializeObject<Rootobject>(json);
            List<Movie> featured = f.results.ToList();
            return featured;

        }
    }
}
