using MessagePack;
using TavernHelios.AdministrationService.ClickHouse.Interfaces;

namespace TavernHelios.AdministrationService.ClickHouse.Dto
{
    [MessagePackObject]
    public class LogMessagePack : IEntity
    { 
        [Key(0)]
        public DateTime Timestamp { get; set; }

        [Key(1)]
        public string Level { get; set; }

        [Key(2)]
        public string MessageTemplate { get; set; }

        [Key(3)]
        public string Method { get; set; }

        [Key(4)]
        public string Path { get; set; }

        [Key(5)]
        public string RequestBody { get; set; }

        [Key(6)]
        public int StatusCode { get; set; }

        [Key(7)]
        public string ResponseBody { get; set; }

        [Key(8)]
        public string Controller { get; set; }

        [Key(9)]
        public string Application { get; set; }

        [Key(10)]
        public string SourceContext { get; set; }

        [Key(11)]
        public string RequestId { get; set; }

        [Key(12)]
        public string RequestPath { get; set; }

        [Key(13)]
        public string ConnectionId { get; set; }

        [Key(14)]
        public string User { get; set; }

        [Key(15)]
        public string Exception { get; set; }
    }
}
