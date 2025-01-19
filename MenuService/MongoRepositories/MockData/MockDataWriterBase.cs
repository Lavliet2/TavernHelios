using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.Entities;
using APICore.Interfaces;
using MongoRepositories.Interfaces;

namespace MongoRepositories.MockData
{
    public abstract class MockDataWriterBase<T> : IDbMockDataWriter<T> where T : IEntity
    {
        protected readonly IRepository<T> _repository;
        protected MockDataWriterBase(
            IRepository<T> repository
            )
        {
            _repository = repository;
        }

        public abstract Task<IEnumerable<T>> FillDbWithMockDataAsync();

        public virtual async Task<bool> IsNeedFillMockDataAsync()
        {
            //TODO: не использовать GetAll для экономии
            var existingData = await _repository.GetAllAsync();
            return !existingData.Any();
        }
    }
}
