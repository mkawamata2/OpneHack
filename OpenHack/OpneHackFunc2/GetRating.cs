using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace OpneHackFunc2
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string ratingId = req.Query["ratingId"];

            MongoClient client = new MongoClient(ConnectionString.Value);
            IMongoDatabase database = client.GetDatabase("Rating");
            IMongoCollection<RatingInfo> collection = database.GetCollection<RatingInfo>("id");

            var result = collection.Find(new BsonDocument { { "id", new Guid(ratingId) } }).FirstOrDefault();

            if (result == null)
            {
                return new NotFoundObjectResult("No data...");
            }

            var jsonResult = JsonConvert.SerializeObject(result);
            log.LogInformation(jsonResult);

            return (ActionResult)new OkObjectResult(jsonResult);
        }
    }
}
