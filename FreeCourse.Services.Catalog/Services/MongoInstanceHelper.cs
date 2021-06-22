using AutoMapper;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class MongoInstanceHelper : IMongoInstanceHelper
    {
        private readonly IMongoDatabase _mongoDatabase;
        public MongoInstanceHelper(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            _mongoDatabase = client.GetDatabase(databaseSettings.DatabaseName);
        }

        public IMongoCollection<T> GetMongoCollectionByName<T>(string collectionName)
        {
            var mongoCollection = _mongoDatabase.GetCollection<T>(collectionName);

            return mongoCollection;
        }
    }
}
