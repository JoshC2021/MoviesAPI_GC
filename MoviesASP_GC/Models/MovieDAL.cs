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
    }
}
