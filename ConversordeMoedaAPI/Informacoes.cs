using System.Text.Json.Serialization;

namespace ConversordeMoedaAPI
{
    internal class Informacoes
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("codein")]
        public string CodeIn { get; set; }

        [JsonPropertyName("high")]
        public string High { get; set; }

        [JsonPropertyName("low")]
        public string Low { get; set; }

        [JsonPropertyName("bid")]
        public string Bid { get; set; }
    }
}