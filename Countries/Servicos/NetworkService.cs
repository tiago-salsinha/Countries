namespace Countries.Servicos
{
    using Modelos;
    using System.Net;
    public class NetworkService
    {
        /// <summary>
        /// Metodo para testar se a aplicação têm conexão à internet ou não
        /// </summary>
       public Response CheckConnection()//retorna um objecto do tipo response que vêm da class Response 
        {
            var client = new WebClient();//o webclient testa se têm ligação à net

            try
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))//faz ping ao servidor da google, é uma forma de testar se existe ligacao à net ou não
                {
                    return new Response
                    {
                        IsSuccess = true,
                        //Message = "Existe conexão à internet"
                    };
                }
            }
            catch //caso ele não consiga fazer a ligação
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Configure a sua conexão à internet"
                };
            }
        }
    }
}
