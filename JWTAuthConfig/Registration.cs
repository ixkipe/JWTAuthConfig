using JWTAuthConfig.Models;

namespace JWTAuthConfig;

public static class Registration {
  public static User CreateUser(this UserDTO request){
    Validation.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

    return new(){
      UserName = request.UserName,
      PasswordHash = passwordHash,
      PasswordSalt = passwordSalt
    };
  }
}