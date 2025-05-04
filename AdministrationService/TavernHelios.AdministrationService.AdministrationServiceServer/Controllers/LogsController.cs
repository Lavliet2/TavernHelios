using Microsoft.AspNetCore.Mvc;
using TavernHelios.AdministrationService.AdministrationServiceServer.Services;
using TavernHelios.AdministrationService.ClickHouse.Entities;

namespace WebAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogService _service;

        public LogsController(LogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<LogEntity>> GetAllLogsLikeEntities()
        {
            return await _service.GetLogsAsync();
        }
    }
}