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
    public class CreateAlbum
    {
        private readonly MongoClient _mongoClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private readonly IMongoCollection<Album> _albums;
        
        public CreateAlbum(
            MongoClient mongoClient,
            ILogger<CreateAlbum> logger,
            IConfiguration config)
        {
            _mongoClient = mongoClient;
            _logger = logger;
            _config = config;

            var database = _mongoClient.GetDatabase(_config[Settings.DATABASE_NAME]);
            _albums = database.GetCollection<Album>(_config[Settings.COLLECTION_NAME]);
        }

        [FunctionName(nameof(CreateAlbum))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CreateAlbum")] HttpRequest req)
        {
            IActionResult returnValue = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var input = JsonConvert.DeserializeObject<Album>(requestBody);

            var album = new Album
            {
                // TODO: Need to Generate this ourselves
                Id = "5bfd996f7b8e48dc15ff215d",
                AlbumName = input.AlbumName,
                Artist = input.Artist,
                Price = input.Price,
                ReleaseDate = input.ReleaseDate,
                Genre = input.Genre
            };

            try
            {
                _albums.InsertOne(album);
                returnValue = new OkObjectResult(album);
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
