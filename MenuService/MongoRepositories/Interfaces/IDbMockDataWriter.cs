using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.ApiCore.Entities;
using TavernHelios.MenuService.ApiCore.Interfaces;

namespace MongoRepositories.Interfaces
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
