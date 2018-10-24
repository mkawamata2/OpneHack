using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OpneHackFunc2
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            CreateRatingIn requestBody,
            ILogger log)
        {
            HttpClient httpClient1 = new HttpClient();
            httpClient1.BaseAddress = new Uri("https://serverlessohlondonuser.azurewebsites.net/");
            httpClient1.DefaultRequestHeaders.Accept.Clear();
            httpClient1.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // ������ API ���Ăяo���� userId �����؂��܂��B
            var getUserResult = await httpClient1.GetAsync($"api/GetUser?userId={requestBody.UserId}");
            if (getUserResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new BadRequestObjectResult($"userId:{requestBody.UserId} is invalid.");
            }

            // ������ API ���Ăяo���� productId �����؂��܂��B
            HttpClient httpClient2 = new HttpClient();
            httpClient2.BaseAddress = new Uri("https://serverlessohlondonproduct.azurewebsites.net/");
            httpClient2.DefaultRequestHeaders.Accept.Clear();
            httpClient2.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var getProductResult = await httpClient2.GetAsync($"api/GetProduct?productid={requestBody.ProductId}");
            if (getProductResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new BadRequestObjectResult($"productId:{requestBody.ProductId} is invalid.");
            }

            MongoClient client = new MongoClient(ConnectionString.Value);

            // DB�����w�肵�āADB���擾
            IMongoDatabase db = client.GetDatabase("Rating");

            // �h�L�������g�̍쐬
            var ratingInfo = new RatingInfo
            {
                Id = Guid.NewGuid(),
                UserId = new Guid(requestBody.UserId),
                ProductId = new Guid(requestBody.ProductId),
                LocationName = requestBody.LocationName,
                Timestamp = DateTimeOffset.UtcNow.ToString(),
                Rating = requestBody.Rating,
                UserNotes = requestBody.UserNotes
            };

            // �R���N�V�������w��(���̏ꍇ�Aarticles�R���N�V����)
            var collection = db.GetCollection<RatingInfo>("id");

            // Insert
            collection.InsertOne(ratingInfo);

            return new OkObjectResult(ratingInfo);
            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }


    public class CreateRatingIn
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string LocationName { get; set; }
        public int Rating { get; set; }
        public string UserNotes { get; set; }
    }

}
