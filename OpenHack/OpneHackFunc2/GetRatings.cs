using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpneHackFunc2
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string userId = req.Query["userId"];

            MongoClient client = new MongoClient(ConnectionString.Value);
            IMongoDatabase database = client.GetDatabase("Rating");
            IMongoCollection<RatingInfo> collection = database.GetCollection<RatingInfo>("id");

            var result = collection.Find(new BsonDocument { { "userId", new Guid(userId) } }).ToList();
            if (result.Count == 0)
            {
                return new NotFoundObjectResult("No data...");
            }

            var jsonResult = JsonConvert.SerializeObject(result);
            log.LogInformation(jsonResult);

            return (ActionResult)new OkObjectResult(jsonResult);
        }
    }
}
