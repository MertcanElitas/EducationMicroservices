using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public interface IMongoInstanceHelper
    {
        IMongoCollection<T> GetMongoCollectionByName<T>(string collectionName);
    }
}
