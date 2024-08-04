public class TokenService : ITokenService
{
    private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0); // будет валиден 30 минут
    public string BuildToken(string key, string issuer, UserDTO user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName), // добавляем имя из DTO
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var sesurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(sesurityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}