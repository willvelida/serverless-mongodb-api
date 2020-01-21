using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoMusic.API.Models;
using MongoMusic.API.Helpers;

namespace MongoMusic.API.Functions
{
    public class GetAllAlbums
    {
        private readonly MongoClient _mongoClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private readonly IMongoCollection<Album> _albums;

        public GetAllAlbums(
            MongoClient mongoClient,
            ILogger<GetAllAlbums> logger,
            IConfiguration config)
        {
            _mongoClient = mongoClient;
            _logger = logger;
            _config = config;

            var database = _mongoClient.GetDatabase(_config[Settings.DATABASE_NAME]);
            _albums = database.GetCollection<Album>(_config[Settings.COLLECTION_NAME]);
        }

        [FunctionName(nameof(GetAllAlbums))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Albums")] HttpRequest req)
        {
            IActionResult returnValue = null;

            try
            {
                var result = _albums.Find(album => true).ToList();

                if (result == null)
                {
                    _logger.LogInformation($"There are no albums in the collection");
                    returnValue = new NotFoundResult();
                }
                else
                {
                    returnValue = new OkObjectResult(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown: {ex.Message}");
                returnValue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return returnValue;
        }
    }
}
