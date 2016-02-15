using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using PersonalWebsite.Areas.CarFinder.Models;

namespace PersonalWebsite.Areas.CarFinder.Controllers
{
    public class CarsController : ApiController
    {
        private const string CONNECTION_STRING = "CarFinderConnection";

        private readonly static HttpClient client = new HttpClient();
        private readonly static Bing.BingSearchContainer bingContainer;
        static CarsController()
        {
            client.BaseAddress = new System.Uri(System.Configuration.ConfigurationManager.ConnectionStrings["NHTSAConnection"].ConnectionString);

            string bingRootUri = System.Configuration.ConfigurationManager.ConnectionStrings["BingConnection"].ConnectionString;
            bingContainer = new Bing.BingSearchContainer(new Uri(bingRootUri));
            string accountKey = System.Configuration.ConfigurationManager.AppSettings["BingAccountKey"];
            bingContainer.Credentials = new System.Net.NetworkCredential(accountKey, accountKey);
        }

        // GET: api/years
        /// <summary>
        /// This action retrieves a list of available car years by car make, model, trim, and/or style.
        /// You can get all available car years by not supplying any parameters.
        /// </summary>
        /// <param name="make">Optional parameter that restricts the result to only years containing that make of car.</param>
        /// <param name="model">Optional parameter that restricts the result to only years containing that model of car.</param>
        /// <param name="trim">Optional parameter that restricts the result to only years containing that trim of car.</param>
        /// <param name="style">Optional parameter that restricts the result to only years containing that style of car.</param>
        /// <returns>IEnumerable list of car years</returns>
        [ActionName("years")]
        public async Task<IHttpActionResult> GetYears(string make = "", string model = "", string trim = "", string style = "")
        {
            using (DbContext db = new DbContext(CONNECTION_STRING))
            {
                var years = await db.Database.SqlQuery<int>("EXEC GetYears @make, @model, @trim, @style",
                    new SqlParameter("make", make), new SqlParameter("model", model),
                    new SqlParameter("trim", trim), new SqlParameter("style", style)).ToArrayAsync();
                if (years != null)
                    return Ok(years);
                else
                    return NotFound();
            }
        }

        // GET: api/makes
        /// <summary>
        /// This action retrieves a list of available car makes by year, model, trim, and/or style.
        /// You can get all available car makes by not supplying any parameters.
        /// </summary>
        /// <param name="year">Optional parameter that restricts the result to only makes from that year.</param>
        /// <param name="model">Optional parameter that restricts the result to only makes containing that model of car.</param>
        /// <param name="trim">Optional parameter that restricts the result to only makes containing that trim of car.</param>
        /// <param name="style">Optional parameter that restricts the result to only makes containing that style of car.</param>
        /// <returns>IEnumerable list of car makes</returns>
        [HttpGet]
        [ActionName("makes")]
        public async Task<IHttpActionResult> GetMakes(int year = 0, string model = "", string trim = "", string style = "")
        {
            using (DbContext db = new DbContext(CONNECTION_STRING))
            {
                var makes = await db.Database.SqlQuery<string>("EXEC GetMakes @year, @model, @trim, @style",
                    new SqlParameter("year", year), new SqlParameter("model", model),
                    new SqlParameter("trim", trim), new SqlParameter("style", style)).ToArrayAsync();
                if (makes != null)
                    return Ok(makes);
                else
                    return NotFound();
            }
        }

        // GET: api/models
        /// <summary>
        /// This action retrieves a list of available car models by year, make, trim, and/or style.
        /// You can get all available car models by not supplying any parameters.
        /// </summary>
        /// <param name="year">Optional parameter that restricts the result to only models from that year.</param>
        /// <param name="make">Optional parameter that restricts the result to only models containing that make of car.</param>
        /// <param name="trim">Optional parameter that restricts the result to only models containing that trim of car.</param>
        /// <param name="style">Optional parameter that restricts the result to only models containing that style of car.</param>
        /// <returns>IEnumerable list of car models</returns>
        [ActionName("models")]
        public async Task<IHttpActionResult> GetModels(int year = 0, string make = "", string trim = "", string style = "")
        {
            using (DbContext db = new DbContext(CONNECTION_STRING))
            {
                var models = await db.Database.SqlQuery<string>("EXEC GetModels @year, @make, @trim, @style",
                    new SqlParameter("year", year), new SqlParameter("make", make),
                    new SqlParameter("trim", trim), new SqlParameter("style", style)).ToArrayAsync();
                if (models != null)
                    return Ok(models);
                else
                    return NotFound();
            }
        }

        // GET: api/trims
        /// <summary>
        /// This action retrieves a list of available car trims by year, make, model, and/or style.
        /// You can get all available car trims by not supplying any parameters.
        /// </summary>
        /// <param name="year">Optional parameter that restricts the result to only trims from that year.</param>
        /// <param name="make">Optional parameter that restricts the result to only trims containing that make of car.</param>
        /// <param name="model">Optional parameter that restricts the result to only trims containing that model of car.</param>
        /// <param name="style">Optional parameter that restricts the result to only trims containing that style of car.</param>
        /// <returns>IEnumerable list of car trims</returns>
        [ActionName("trims")]
        public async Task<IHttpActionResult> GetTrims(int year = 0, string make = "", string model = "", string style = "")
        {
            using (DbContext db = new DbContext(CONNECTION_STRING))
            {
                var trims = await db.Database.SqlQuery<string>("EXEC GetTrims @year, @make, @model, @style",
                    new SqlParameter("year", year), new SqlParameter("make", make),
                    new SqlParameter("model", model), new SqlParameter("style", style)).ToArrayAsync();
                if (trims != null)
                    return Ok(trims);
                else
                    return NotFound();
            }
        }

        // GET: api/styles
        /// <summary>
        /// This action retrieves a list of available car styles by year, make, model, and/or trim.
        /// You can get all available car styles by not supplying any parameters.
        /// </summary>
        /// <param name="year">Optional parameter that restricts the result to only styles from that year.</param>
        /// <param name="make">Optional parameter that restricts the result to only styles containing that make of car.</param>
        /// <param name="model">Optional parameter that restricts the result to only styles containing that model of car.</param>
        /// <param name="trim">Optional parameter that restricts the result to only styles containing that trim of car.</param>
        /// <returns>IEnumerable list of car trims</returns>
        [ActionName("styles")]
        public async Task<IHttpActionResult> GetStyles(int year = 0, string make = "", string model = "", string trim = "")
        {
            using (DbContext db = new DbContext(CONNECTION_STRING))
            {
                var styles = await db.Database.SqlQuery<string>("EXEC GetStyles @year, @make, @model, @trim",
                    new SqlParameter("year", year), new SqlParameter("make", make),
                    new SqlParameter("model", model), new SqlParameter("trim", trim)).ToArrayAsync();
                if (styles != null)
                    return Ok(styles);
                else
                    return NotFound();
            }
        }

        // GET: api/cars
        /// <summary>
        /// This action retrieves a list of available cars by year, make, model, and/or trim.
        /// You can get all available cars by not supplying any parameters, albeit without recalls.
        /// </summary>
        /// <param name="year">Optional parameter that restricts the result to only cars from that year.</param>
        /// <param name="make">Optional parameter that restricts the result to only cars of that make.</param>
        /// <param name="model">Optional parameter that restricts the result to only cars of that model.</param>
        /// <param name="trim">Optional parameter that restricts the result to only cars of that trim.</param>
        /// <param name="style">Optional parameter that restricts the result to only cars of that style.</param>
        /// <returns>IEnumerable list of cars; Returns 404 if no cars are found.</returns>
        [ActionName("cars")]
        public async Task<IHttpActionResult> GetCars(int year = 0, string make = "", string model = "", string trim = "", string style = "")
        {
            // Asynchronously gets cars from local database.
            List<Car> cars;
            using (DbContext db = new DbContext(CONNECTION_STRING))
            {
                // Waits for the results from the local database
                cars = await db.Database.SqlQuery<Car>("EXEC GetCars @year, @make, @model, @trim, @style",
                    new SqlParameter("year", year), new SqlParameter("make", make), new SqlParameter("model", model),
                    new SqlParameter("trim", trim), new SqlParameter("style", style)).ToListAsync();
            }
            // CURRENTLY DISABLED TO IMPROVE QUERY SPEED
            /*
            // This would get one set of more generic images: Task<IEnumerable<ImageResult>> imagesTask = GetImages(year, make, model, trim);
            // This method instead gets a list of tasks which will asynchronously populate the cars' imageUrls
            // If the number of cars found is more than 5, then no image data is retrieved. This is to limit the max number
            //     of Bing search queries that can be made per car search.
            List<Task> imageTasks = new List<Task>();
            List<Task> recallTasks = new List<Task>();
            if (cars.Count <= 5)
                cars.ForEach(car => {
                    // Asynchronously starts a separate task to find images for each car, and stores the tasks in the imageTasks list.
                    imageTasks.Add(Task.Run(async () =>
                        car.ImageUrls = (await GetImages(car.Model_Year, car.Make_Display, car.Model_Name,
                                                         car.Model_Trim, imagesPerCar)).ToArray()));
                // Asynchronously starts a separate task to find recalls for each car, and stores the tasks in the recallTasks list.
                    recallTasks.Add(Task.Run(async () =>
                        car.Recalls = (await GetRecalls(car.Model_Year, car.Make_Display, car.Model_Name)).ToArray()));
                });
            // Asynchronously waits until all of the data for the cars has been retrieved and populated by the various Tasks.
            await Task.WhenAll(Task.WhenAll(imageTasks), Task.WhenAll(recallTasks));
             */

            // Returns the cars data as a JSON string.
            return Json(cars);
        }

        // GET: api/recalls
        /// <summary>
        /// This action retrieves a list of car recalls by year, make, and model.
        /// </summary>
        /// <param name="year">Restricts the result to only recalls from that year.</param>
        /// <param name="make">Restricts the result to only recalls for that make of car.</param>
        /// <param name="model">Restricts the result to only recalls for that model of car.</param>
        /// <returns>IEnumerable list of car recalls</returns>
        [ActionName("recalls")]
        public async Task<Recall[]> GetRecalls(string year, string make, string model)
        {
            using (var responseStream = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/"
                    + year + "/make/" + make + "/model/" + model + "?format=json"))
            {
                RecallsAPI result = await responseStream.Content.ReadAsAsync<RecallsAPI>();
                return result.Results;
            }
        }

        // GET: api/images
        /// <summary>
        /// This action retrieves a list of Bing image thumbnail URLs by car year, make, model, and/or trim.
        /// You will get no results if you supply no parameters, and you may get results unrelated to cars
        /// if you supply only one or two parameters.
        /// </summary>
        /// <param name="year">Optional parameter that queries for images based on year.</param>
        /// <param name="make">Optional parameter that queries for images based on make.</param>
        /// <param name="model">Optional parameter that queries for images based on model.</param>
        /// <param name="trim">Optional parameter that queries for images based on trim.</param>
        /// <param name="count">Optional parameter that sets the number of images returned. The default value is 5.</param>
        /// <returns>IEnumerable list of images</returns>
        [ActionName("images")]
        public async Task<List<string>> GetImages(string year = "", string make = "", string model = "", string trim = "", int count = 5)
        {
            string query = "\"" + year + " " + make + " " + model + " " + trim + "\"";
            // Build the query, with the number of results restricted by the count parameter.
            System.Data.Services.Client.DataServiceQuery<Bing.ImageResult> imageQuery =
                bingContainer.Image(query, null, "en-us", null, null, null, null).AddQueryOption("$top", count);
            // Asynchronously executes the image search query
            List<string> results = (await Task.Factory.FromAsync<IEnumerable<Bing.ImageResult>>(imageQuery.BeginExecute, imageQuery.EndExecute, null))
                .Select(result => result.Thumbnail.MediaUrl).ToList();
            // If too few images were found with this specific query, then it reruns the query with broader search parameters to acquire more images.
            // Only one additional query will ever be made in this way, and even then only if car trim was supplied as a parameter.
            if (!String.IsNullOrWhiteSpace(trim) && results.Count < count)
                results.AddRange(await GetImages(year, make, model, "", count - results.Count));
            return results;
        }
    }
}
