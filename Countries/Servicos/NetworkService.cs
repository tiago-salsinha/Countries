namespace Countries.Servicos
{
    using Modelos;
    using System.Net;
    public class NetworkService
    {
        /// <summary>
        /// Method to check if there is internet connection
        /// </summary>
       public Response CheckConnection()
        {
            var client = new WebClient();

            try
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSuccess = true,                       
                    };
                }
            }
            catch
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please connect your device to the internet."
                };
            }
        }
    }
}
