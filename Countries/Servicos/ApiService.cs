namespace Countries.Servicos
{
    using Modelos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApiService
    {
        //Metodo para ir busacar as taxas:
        public async Task<Response> GetRates(string urlBase, string controller)//um metodo asincrono que vai devolver um objecto do tipo Response 
        {
            try
            {
                var client = new HttpClient();//liga-se à Api
                client.BaseAddress = new Uri(urlBase);//liga-se à base

                var response = await client.GetAsync(controller);//liga-se ao controlador

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)//testa caso a api funciona ou não
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var rates = JsonConvert.DeserializeObject<List<RegionalBloc>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = rates
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
