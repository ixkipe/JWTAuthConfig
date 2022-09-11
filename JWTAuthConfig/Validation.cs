using System.Security.Cryptography;
using System.Text;
using JWTAuthConfig.Models;

namespace JWTAuthConfig;

public static class Validation {
  public static bool ValidUser(UserDTO request, IEnumerable<User> users){
    var foundUser = users.Where(x => x.UserName == request.UserName).First();
    if (foundUser is null) return false;

    return ValidPasswordHash(request.Password, foundUser.PasswordHash, foundUser.PasswordSalt);
  }
  public static bool ValidUser(UserDTO request, User user){
    if (user.UserName != request.UserName) return false;

    return ValidPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
  }
  public static bool ValidUser(UserDTO request, IQueryable<User> users){
    var foundUser = users.Where(x => x.UserName == request.UserName).First();
    if (foundUser is null) return false;

    return ValidPasswordHash(request.Password, foundUser.PasswordHash, foundUser.PasswordSalt);
  }

  private static bool ValidPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt){
    using (HMACSHA512 hmac = new(passwordSalt)){
      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      return computedHash.SequenceEqual(passwordHash);
    }
  }

  public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt){
    using (HMACSHA512 hmac = new()){
      passwordSalt = hmac.Key;
      passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
  }
}