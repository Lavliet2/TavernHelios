using MessagePack;

namespace TavernHelios.MessagePack
{
    [MessagePackObject]
    public class Message
    {
        [Key(0)]
        public string Content { get; set; }

        [Key(1)]
        public DateTime Timestamp { get; set; }
    }
}
