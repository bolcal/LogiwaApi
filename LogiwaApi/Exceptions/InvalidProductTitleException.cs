namespace LogiwaApi.Exceptions
{
    public class InvalidProductTitleException :Exception
    {
        public InvalidProductTitleException(string message) : base(message)
        {
        }
        public InvalidProductTitleException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
