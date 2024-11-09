﻿using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace LemaxHotelSearchApi.Security
{
    public class JwtAuthenticationManager
    {
        private readonly string _key;
        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        {
            {"test", "password" },
            {"test1", "pwd" }
        };

        public JwtAuthenticationManager(string key)
        {
            _key = key;
        }

        public string Authenticate(string username, string password)
        {
            if (!users.Any(u => u.Key == username && u.Value == password))
            {
                return null;
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
