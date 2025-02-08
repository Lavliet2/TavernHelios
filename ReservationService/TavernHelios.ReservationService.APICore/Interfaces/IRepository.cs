using System.Linq.Expressions;

namespace TavernHelios.ReservationService.ApiCore.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Получить сущность по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(long id);

        /// <summary>
        /// Получить сущность по условию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetByQueryAsync(Expression<Func<T, bool>> condition);

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
        Task<long> DeleteAsync(long entityId);

    }
}
