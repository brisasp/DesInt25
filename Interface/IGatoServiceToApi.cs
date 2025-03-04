using LoginRegister.Models;


namespace LoginRegister.Interface
{
    public interface IGatoServiceToApi
    {
        // Obtiene un GATO desde la API
        Task<IEnumerable<GatoDTO>> GetGatos();

        // Agrega un GATO a la API
        Task PostGato(GatoDTO Gato);

        // Modifica un GATO ya existente
        Task PutGato(GatoDTO Gato);
    }
}
