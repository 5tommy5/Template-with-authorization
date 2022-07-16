using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class LoginInfoModel
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("tokens")]
        public TokenModel? Tokens { get; set; }

        [JsonPropertyName("response")]
        public ResponseModel Response { get; set; }

        public LoginInfoModel(string username, TokenModel tokens)
        {
            Username = username;
            Tokens = tokens;
            Response = new ResponseModel()
            {
                Errors = null,
                Success = true
            };
        }

        public LoginInfoModel(params string[] errors)
        {
            Username = null;
            Tokens = null;
            Response = new ResponseModel();
            Response.Errors = new List<string>(errors);

            Response.Success = false;
        }
    }
}
