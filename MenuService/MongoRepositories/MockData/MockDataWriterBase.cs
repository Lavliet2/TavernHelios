using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Interfaces;
using TavernHelios.MenuService.MongoRepositories.Interfaces;

namespace TavernHelios.MenuService.MongoRepositories.MockData
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
            //Всегда вернуть true
            return await Task.FromResult(true);

            ////Если нет данный, вернуть true
            //var existingData = await _repository.GetAllAsync();
            //return !existingData.Any();
        }
    }
}
