using System.Linq;
using System.Linq.Expressions;
using TavernHelios.ReservationService.ApiCore.Interfaces;
using TavernHelios.ReservationService.APICore.Entities;

namespace TavernHelios.ReservationService.PostgreRepository
{
    /// <summary>
    /// Заглушка
    /// TODO подрутить EF и коненкт к постгре
    /// </summary>
    public class ReservationRepository_Mock : IRepository<ReservationEntity>
    {

        private Dictionary<long, ReservationEntity> _reservations = new Dictionary<long,ReservationEntity>();
        private List<DishReservationEntity> _dishReservations = new List<DishReservationEntity>();
        
        public async Task<ReservationEntity> CreateAsync(ReservationEntity entity)
        {
            return await Task.Factory.StartNew<ReservationEntity>(() =>
            {
                entity.Id = LongRandom();

                //Пример того как это будет храниться в реальной БД ( в двух таблицах со связью один ко многим )
                foreach (var dish in entity.DishIds)
                {
                    var dishReservation = new DishReservationEntity()
                    {
                        DishId = dish,

                        //Следующие поля EF должна заполнять автоматом
                        Id = LongRandom(),
                        Reservation = entity,
                        ReservationId = entity.Id
                    };
                    _dishReservations.Add(dishReservation);
                }
                _reservations[entity.Id] = entity;

                return entity;
            });
        }

        public async Task<long> DeleteAsync(long entityId)
        {
            _reservations.Remove(entityId);
            return await Task.FromResult(entityId);
        }

        public async Task<IEnumerable<ReservationEntity>> GetByQueryAsync(Expression<Func<ReservationEntity, bool>> condition)
        {
            var func = condition.Compile();
            var queryResult = _reservations.Values.Where(func);
            return await Task.FromResult(queryResult);
        }

        public async Task<ReservationEntity> GetByIdAsync(long id)
        {
            var queryResult = _reservations[id];
            return await Task.FromResult(queryResult);
        }

        public async Task<ReservationEntity> UpdateAsync(ReservationEntity entity)
        {
            _reservations[entity.Id] = entity;
            return await Task.FromResult(entity);
        }

        private long LongRandom()
        {
            Random rand = new Random();
            return rand.NextInt64();
        }
    }
}
