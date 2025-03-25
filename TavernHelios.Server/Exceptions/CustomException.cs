namespace TavernHelios.Server.Exceptions
{
    public class CustomException : Exception
    {
        public string Description { get; set; }

        public CustomException(string description)
        {
            Description = description;
        }
    }
}
