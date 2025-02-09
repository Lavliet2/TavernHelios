using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.ReservationService.ApiCore.Interfaces;
using TavernHelios.ReservationService.APICore.Entities;

namespace TavernHelios.ReservationService.PostgreRepository.Repositories.EF
{
    public class ReservationRepository : EfRepositoryBase<ReservationEntity>
    {
        public Task<ReservationEntity> CreateAsync(ReservationEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<long> DeleteAsync(long entityId)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationEntity> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReservationEntity>> GetByQueryAsync(Expression<Func<ReservationEntity, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationEntity> UpdateAsync(ReservationEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
