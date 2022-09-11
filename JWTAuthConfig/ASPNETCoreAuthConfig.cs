using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Text;
using Microsoft.Extensions.Configuration;
using JWTAuthConfig.Models;

namespace JWTAuthConfig;

public static class ASPNETCoreAuthConfig {
  public static void ConfigureSwaggerOptions(this SwaggerGenOptions options){
    options.AddSecurityDefinition("oauth2", new(){
      Description = "accepts JWT token in \"bearer {token}\" format",
      In = ParameterLocation.Header,
      Name = "Authorization",
      Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
  }

  public static void ConfigureJWTAuthentication(this WebApplicationBuilder builder, string appSettingsJsonTokenSection){
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => {
        options.TokenValidationParameters = new(){
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection(appSettingsJsonTokenSection).Value)
          ),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
  }

  public static string GenerateJWT(this User user, IConfiguration configuration, string appSettingsJsonTokenSection, DateTime expirationDate){
    List<Claim> claims = new(){
      new Claim(ClaimTypes.Name, user.UserName)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(appSettingsJsonTokenSection).Value));

    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var token = new JwtSecurityToken(
      claims: claims,
      expires: expirationDate > DateTime.Now? expirationDate : DateTime.Now.AddDays(7),
      signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}