using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoMusic.API.Functions;
using MongoMusic.API.Helpers;
using MongoMusic.API.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MongoMusic.Tests.FunctionTests
{
    public class CreateAlbumTests
    {
        private Mock<ILogger<CreateAlbum>> _loggerMock;
        private Mock<IConfiguration> _configMock;
        private Mock<MongoClient> _mongoClientMock;

        private Mock<IMongoCollection<Album>> _albumCollectionMock;

        private CreateAlbum _func;

        public CreateAlbumTests()
        {
            _loggerMock = new Mock<ILogger<CreateAlbum>>(MockBehavior.Strict);
            _mongoClientMock = new Mock<MongoClient>(MockBehavior.Strict);
            _configMock = new Mock<IConfiguration>(MockBehavior.Strict);
            _configMock.Setup(x => x[Settings.MONGO_CONNECTION_STRING]).Returns("mongodbconnection");
            _configMock.Setup(x => x[Settings.DATABASE_NAME]).Returns("dbname");
            _configMock.Setup(x => x[Settings.COLLECTION_NAME]).Returns("collectionName");

            _albumCollectionMock = new Mock<IMongoCollection<Album>>(MockBehavior.Strict);

            _func = new CreateAlbum(
                _mongoClientMock.Object,
                _loggerMock.Object,
                _configMock.Object);
        }

        [Fact]
        public async Task can_create_album()
        {
            // Create Fake Album to persist
            var fakeAlbum = new Album
            {
                AlbumName = "AlbumName",
                Artist = "AlbumArtist",
                Genre = "Genre",
                Price = 10.99,
                ReleaseDate = DateTime.Now
            };

            var mockRequestBody = fakeAlbum.ToString();

            //await _func.Run(mockRequestBody)

            // Insert into mock

            // Assert that it persisted
        }

        [Fact]
        public async Task can_throw_500_error()
        {
            // Overwrite a setting in mock

            // Create Fake Album

            // Insert into Mock Client

            // Assert it failed.
        }
    }
}
