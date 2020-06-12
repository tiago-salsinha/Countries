namespace Countries.Modelos
{
    /// <summary>
    /// Class used to deal with the app response, if the connection is successful, if not throws a detailed error message
    /// </summary>
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
    }
}
