using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.ApiCore.Entities;

namespace TavernHelios.MenuService.ApiCore.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Получить все сущности
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Получить сущность по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Создать сущность
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        /// Изменить сущность
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<string> DeleteAsync(string entityId);

        /// <summary>
        /// Удалить ВСЁ
        /// </summary>
        /// <returns></returns>
        Task<long> DeleteAll();

    }
}
