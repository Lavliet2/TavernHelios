using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Interfaces;

namespace TavernHelios.MenuService.MongoRepositories.Interfaces
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
