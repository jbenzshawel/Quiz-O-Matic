using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ApiQuizGenerator.AppClasses
{
    public class TokenProviderOptions
    {   
        public string Issuer { get; set; }
 
        public string Audience { get; set; }
 
        /// <summary>
        /// Default Token time expiration of 30 minutes
        /// </summary>
        /// <returns></returns>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(30);
 
        public SigningCredentials SigningCredentials { get; set; }
    }

    public interface ITokenProvider 
    {
        TokenValidationParameters ValidationParameters { get; }

        bool ValidateToken(string tokenString);

        string GenerateToken(HttpContext context, string username);

    }

    public class TokenProvider : ITokenProvider
    {
        /// <summary>
        /// Key for encryption
        /// </summary>
        private const string _secretKey = "as71ds94de8asd76ad7945c63ce";
        
        /// <summary>
        /// Secret key byte array
        /// </summary>
        private byte[] _keyBytes = Encoding.UTF8.GetBytes(_secretKey);


        /// <summary>
        /// Issuer string for JWT token 
        /// </summary>
        private const string _issuer = "QuizOMaticIssuer";

        // audience string for JWT token
        private const string _audience = "QuizOMaticAudience";

        /// <summary>
        /// Field for TokenProviderOptions
        /// </summary>
        private readonly TokenProviderOptions _options;
        

        /// <summary>
        /// Validated JWT token
        /// </summary>
        /// <returns></returns>
        internal SecurityToken _ValidToken { get; set; }
        
        /// <summary>
        /// Security Key for JWT 
        /// </summary>
        /// <returns></returns>
        internal SymmetricSecurityKey SigningKey { get; } =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
        
        /// <summary>
        /// Returns TokenValidationParameters for JWT token
        /// </summary>
        public TokenValidationParameters ValidationParameters 
        { 
            get 
            {
                return new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SigningKey,
                
                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                
                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = _audience,
                
                    // Validate the token expiry
                    ValidateLifetime = true,
                
                    // If you want to allow a certain amount of clock drift, set that here:
                    ClockSkew = TimeSpan.Zero
                };
            }
        }

        public TokenProvider() {
            _options = new TokenProviderOptions{
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(_keyBytes), 
                        Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
        }

        /// <summary>
        /// Validates a string JWT and sets this._ValidToken if token is valid
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenString) 
        {
            JwtSecurityToken tokenReceived = new JwtSecurityToken(tokenString);

            if (_keyBytes.Length < 64 && tokenReceived.SignatureAlgorithm == "HS256")
            {
                Array.Resize(ref _keyBytes, 64);
            }
            
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validToken = null;
            try 
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(tokenString, ValidationParameters, out validToken);                
                _ValidToken = validToken;
                return claimsPrincipal.Identity.IsAuthenticated && validToken != null;    
            }
            catch (Exception) 
            {
                return false;            
            }        
        }

        /// <summary>
        /// Generates a JWT 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GenerateToken(HttpContext context, string username)
        {
            string encodedJwt = null;
            var now = DateTime.UtcNow;

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (now.ToUniversalTime() - epoch).TotalSeconds;
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, unixDateTime.ToString(), ClaimValueTypes.Integer64)
            };
            
            try 
            {
                // Create the JWT and write it to a string
                var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(_options.Expiration),
                    signingCredentials: _options.SigningCredentials
                );
                encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            }
            catch (Exception) 
            {
                encodedJwt = string.Empty;
            }
           
            return encodedJwt;
        }
    }
}