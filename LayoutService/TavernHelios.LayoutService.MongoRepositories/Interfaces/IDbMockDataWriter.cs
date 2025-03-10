using TavernHelios.LayoutService.Common.Entities;

namespace TavernHelios.LayoutService.MongoRepositories.Interfaces
{
    public interface IDbMockDataWriter<T> where T : IEntity
    {
        /// <summary>
        /// Нужно ли заполнять таблицу тестовыми данными
        /// </summary>
        /// <returns></returns>
        Task<bool> IsNeedFillMockDataAsync();

        /// <summary>
        /// Заполнить таблицу тестовыми данными
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> FillDbWithMockDataAsync();
    }
}
