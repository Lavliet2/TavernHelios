using TavernHelios.AdministrationService.APICore.Interfaces;

namespace TavernHelios.AdministrationService.APICore.Entities
{
    public class LogEntity : IEntity
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string MessageTemplate { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string RequestBody { get; set; }
        public int StatusCode { get; set; }
        public string ResponseBody { get; set; }
        public string Controller { get; set; }
        public string Application { get; set; }
        public string SourceContext { get; set; }
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
        public string ConnectionId { get; set; }
        public string User { get; set; }
        public string Exception { get; set; }
    }
}
