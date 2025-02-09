using TavernHelios.Server.Data.Models;

namespace TavernHelios.Server.DTO
{
    public class HeliosUserDTO
    {
        public HeliosUserDTO()
        {
        }

        public HeliosUserDTO(HeliosUser user)
        {
            Id = user.Id;
            YandexFullname = user.YandexFullname;
            YandexUserId = user.YandexUserId;
            YandexEmail = user.YandexEmail;
            YandexLogin = user.YandexLogin;
        }

        public int? Id { get; set; }
        public string YandexFullname { get; set; }
        public string YandexUserId { get; set; }
        public string YandexEmail { get; set; }
        public string YandexLogin { get; set; }

    }
}
