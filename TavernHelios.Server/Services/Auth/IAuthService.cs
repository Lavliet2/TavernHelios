using TavernHelios.Server.DTO;

namespace TavernHelios.Server.Services.Auth
{
    public interface IAuthService
    {
        HeliosUserDTO LoginAndRegister(HeliosUserDTO userDTO);
    }
}
