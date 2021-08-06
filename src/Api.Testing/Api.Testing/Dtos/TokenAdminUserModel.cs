using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.Testing.Dtos
{
    public class TokenAdminUserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }
        public List<string> StoreIds { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}