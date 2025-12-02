
using System.Text.Json.Serialization;

namespace JobTracker.Application.DTO
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
