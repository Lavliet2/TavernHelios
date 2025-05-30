﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Interfaces;
using TavernHelios.MenuService.Common.Settings;

namespace TavernHelios.MenuService.MongoRepositories
{
    //Перед запуском - запустить MongoDB.
    //Например в докере: docker run --name myMongoTestDb -p 27018:27018 -d mongo

    public abstract class BaseMongoRepository<T> : IRepository<T> where T: class, IEntity
    {
        protected readonly string _dbName;
        protected readonly string _dbCollectionName;
        protected readonly MongoClient _client;
        protected readonly IMongoDatabase _database;

        protected IMongoCollection<T>? _collection;

        //TODO: обработка ошибок. Пока что просто отдаем пустое значение, если ничего не нашли в БД по id

        public BaseMongoRepository(
            IOptions<MenuMongoConnectionSettings> mongoSettings
            )
        {
            //Обозначаем Id в качестве идентифицирующей проперти в БД
            BsonClassMap.RegisterClassMap<T>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.SetIdMember(
                cm.MapIdProperty(c => ((T)c).Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                );
            });

            var connectionString = mongoSettings.Value.GetConnectionString();
            _client = new MongoClient( connectionString );
            _dbName = mongoSettings.Value.DbName;
            _dbCollectionName = typeof(T).Name;
            _database = _client.GetDatabase(_dbName);

            CheckConnection();
        }

        private void CheckConnection()
        {
            bool isMongoLive = _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);

            if (isMongoLive)
            {
                Console.WriteLine($"{_dbName} Connected");
            }
            else
            {
                Console.WriteLine($"{_dbName} NOT Connected");
            }
        }

       
        public virtual async Task<T> CreateAsync(T entity)
        {
            var collection = await GetCollectionAsync(_dbCollectionName);
            try
            {
                await collection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return entity;
        }

        public virtual async Task<string> DeleteAsync(string entityId)
        {
            var entity = await this.GetByIdAsync(entityId);
            if (entity == null)
                return string.Empty;

            entity.IsDeleted = true;
            var deletedEntity = await this.UpdateAsync(entity);
            return deletedEntity?.Id ?? string.Empty;

        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var allEntities = await (await GetCollectionAsync(_dbCollectionName)).Find("{}").ToListAsync();
            return allEntities;
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            var entity = await (await GetCollectionAsync(_dbCollectionName))
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();

            return entity;
        }
        public virtual async Task<T> UpdateAsync(T entity)
        {
            var user = await (await GetCollectionAsync(_dbCollectionName))
                .FindOneAndReplaceAsync(p => p.Id == entity.Id, entity, new() { ReturnDocument = ReturnDocument.After });
            return user;
        }


        //Удалить ВСЁ
        public virtual async Task<long> DeleteAll()
        {
            var res = await (await GetCollectionAsync(_dbCollectionName))
                .DeleteManyAsync(x => true);
                
            return res.DeletedCount;
        }

        private async Task<IMongoCollection<T>> GetCollectionAsync(string collectionName)
        {
            if (_collection != null)
                return _collection;
            else
            {
                _collection = await InitCollection(collectionName);
                return _collection;
            }
        }

        private async Task<IMongoCollection<T>> InitCollection(string collectionName)
        {
            var collectionNames = await _database.ListCollectionNamesAsync();
            if (collectionNames.ToList().FirstOrDefault(x => x == _dbCollectionName) != null)
            {
                return _database.GetCollection<T>(_dbCollectionName);
            }
            else
            {
                await _database.CreateCollectionAsync(_dbCollectionName);
                return _database.GetCollection<T>(_dbCollectionName);
            }
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Func<T, bool> condition)
        {
            return (await GetCollectionAsync(_dbCollectionName))
                .AsQueryable()
                .Where(condition)
                .ToList();
        }
    }
}
