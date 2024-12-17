﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdApp.Core.Helpers
{
    public class AuthorizationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;


        public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.ToString().Contains("/login") || 
                context.Request.Path.ToString().Contains("/register"))
            {
                await _next(context);
                return;
            }
           
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

          
            if (string.IsNullOrEmpty(token))
            {
                token = context.Request.Cookies["jwt"];
            }

            if (!string.IsNullOrEmpty(token))
            {
                if (ValidateToken(token, context))
                {
                    await _next(context); // Continue down the pipeline if the token is valid
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
                    await context.Response.WriteAsync("Invalid Token");
                }
            }
            else
            {
                // No token found
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
                await context.Response.WriteAsync("Token is required");
            }
        }

        private bool ValidateToken(string token, HttpContext context)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero  // Remove default clock skew
                }, out SecurityToken validatedToken);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Validate the signature of the token
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Secret key for validation
                    ValidateIssuer = false, // Skip Issuer validation (set to true if needed)
                    ValidateAudience = false, // Skip Audience validation (set to true if needed)
                    ClockSkew = TimeSpan.Zero // No clock skew for expiration validation
                };
                
                // Validate the token and return claims
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                if (validatedToken is JwtSecurityToken jwtToken &&
              jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principal.Claims.Select(x => x.Type == "id").FirstOrDefault();
                    context.Items.Add("userName",principal.Identity.Name);
                }
                    // Additional custom validation logic can go here

                    return true; // Return true if token is valid
            }
            catch
            {
                return false; // Return false if validation fails
            }

        }
    }

}