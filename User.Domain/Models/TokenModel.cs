﻿using System.Text.Json.Serialization;

namespace Users.Domain.Models
{
    public class TokenModel
    {
        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
        
    }
}
