using System.Text.Json.Serialization;


namespace LoginRegister.Models
{
    public class GatoDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("pais")]
        public string Pais { get; set; }

        [JsonPropertyName("Image")]
        public string Image { get; set; }

    }
    }

  
  





