namespace TavernHelios.AdministrationService.APICore.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        ///// <summary>
        ///// Получить сущность по timestamp
        ///// </summary>
        ///// <param name="timestamp"></param>
        ///// <returns></returns>
        //Task<T> GetByIdAsync(DateTime timestamp);

        ///// <summary>
        ///// Получить сущность по условию
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<IEnumerable<T>> GetByQueryAsync(Func<T, bool> condition);

        /// <summary>
        /// Создать сущность
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T?> CreateAsync(T entity);
    }
}
