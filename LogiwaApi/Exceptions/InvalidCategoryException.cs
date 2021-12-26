namespace LogiwaApi.Exceptions
{
    public class InvalidCategoryException : Exception
    {
        public InvalidCategoryException(string message):base(message)
        {
        }
        public InvalidCategoryException(string message, Exception innerException):base(message,innerException)
        {
        }

    }
}
