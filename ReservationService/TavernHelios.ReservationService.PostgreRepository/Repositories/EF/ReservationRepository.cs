using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TavernHelios.ReservationService.ApiCore.Interfaces;
using TavernHelios.ReservationService.APICore.Entities;

namespace TavernHelios.ReservationService.PostgreRepository.Repositories.EF
{
    public class ReservationRepository : EfRepositoryBase<ReservationEntity>
    {
        protected readonly DbSet<DishReservationEntity> _dishSet;
        public ReservationRepository(DbContext context) : base(context)
        {
            _dishSet = context.Set<DishReservationEntity>();
        }

        public override Task<ReservationEntity> CreateAsync(ReservationEntity entity)
        {
            return base.CreateAsync(entity);
        }

        public override async Task<long> DeleteAsync(long entityId)
        {
            var reservation = await GetByIdAsync(entityId);
            if (reservation == null)
            {
                //Минимальная валидация
                Console.WriteLine($"Error: (Delete) Reservation with id {entityId} does not exist");
                return -1;
            }
            reservation.IsDeleted = true;
            var deleted = await UpdateInnerAsync(reservation, false);
            return deleted?.Id ?? -1;
        }

        public override Task<ReservationEntity> GetByIdAsync(long id)
        {
            return base.GetByIdAsync(id);
        }

        public override async Task<ReservationEntity> UpdateAsync(ReservationEntity entity)
        {
            return await UpdateInnerAsync(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="needUpdateDishLish"></param>
        /// <returns></returns>
        private async Task<ReservationEntity> UpdateInnerAsync(ReservationEntity entity, bool needUpdateDishLish = true)
        {
            var existingDishBinds = _dishSet.Where(x => x.ReservationId == entity.Id).ToList();
            if (existingDishBinds.Any() && needUpdateDishLish)
            {
                _dishSet.RemoveRange(existingDishBinds);
            }
            return await base.UpdateAsync(entity);
        }

        protected override IEnumerable<ReservationEntity> GetQuerySet()
        {
            return _entitySet.Include(x => x.DishReservations);
        }
    }
}
