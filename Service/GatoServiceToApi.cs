using LoginRegister.Helpers;
using LoginRegister.Interface;
using LoginRegister.Models;
using LoginRegister.Services;



namespace LoginRegister.Service
{

   public class GatoServiceToApi : IGatoServiceToApi
   {
        private readonly IHttpJsonProvider<GatoDTO> _httpJsonProvider;
      

        public GatoServiceToApi(IHttpJsonProvider<GatoDTO>  httpJsonProvider) 
        {
            _httpJsonProvider = httpJsonProvider;
        }


        //aqui has cambiado Gato a gatoS
         public async  Task<IEnumerable<GatoDTO>> GetGatos()
         {
 
            IEnumerable<GatoDTO> gatos = await _httpJsonProvider.GetAsync(Constants.GATO_URL);

         return gatos;
         }

        public Task<IEnumerable<GatoDTO>> GetGato()
        {
           throw new NotImplementedException();
       }

        public async Task PostGato(GatoDTO gato)
            {
                try
                {
                    if (gato == null) return;
                    var response = await _httpJsonProvider.PostAsync(Constants.GATO_URL, gato);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        public async Task PutGato(GatoDTO gato)
        {
            try
            {
                if (gato == null) return;
                var response = await _httpJsonProvider.PutAsync(Constants.GATO_URL + "/" + gato.Id, gato);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
   
}