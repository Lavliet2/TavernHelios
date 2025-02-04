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

        private List<ReservationEntity> _reservations = new List<ReservationEntity>();
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
                _reservations.Add(entity);

                return entity;
            });
        }

        public async Task<long> DeleteAll()
        {
            throw new NotImplementedException();
        }

        public async Task<string> DeleteAsync(long entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReservationEntity>> GetAllAsync()
        {
            return await Task.Factory.StartNew<IEnumerable<ReservationEntity>>(() =>
            {
                return _reservations.ToArray();
            });
        }

        public async Task<ReservationEntity> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<ReservationEntity> UpdateAsync(ReservationEntity entity)
        {
            throw new NotImplementedException();
        }

        private long LongRandom()
        {
            Random rand = new Random();
            return rand.NextInt64();
        }
    }
}
