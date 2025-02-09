using Microsoft.EntityFrameworkCore;
using TavernHelios.Server.Data;
using TavernHelios.Server.Data.Models;
using TavernHelios.Server.DTO;

namespace TavernHelios.Server.Services.Auth
{
    public class YandexAuthService : IAuthService
    {
        private readonly EfDbContext _dbContext;
        public YandexAuthService(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public HeliosUserDTO LoginAndRegister(HeliosUserDTO userDTO)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(x => x.YandexUserId == userDTO.YandexUserId);
            if (existingUser != null)
                return new HeliosUserDTO(existingUser);

            var newUser = new HeliosUser
            {
                YandexFullname = userDTO.YandexFullname,
                YandexUserId = userDTO.YandexUserId,
                YandexEmail = userDTO.YandexEmail,
                YandexLogin = userDTO.YandexLogin
            };

            _dbContext.Add(newUser);
            _dbContext.SaveChanges();

            // TEMP
            return new HeliosUserDTO(newUser);
        }
    }
}
