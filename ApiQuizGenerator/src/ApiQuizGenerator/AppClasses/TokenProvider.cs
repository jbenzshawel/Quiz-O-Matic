using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;

namespace ApiQuizGenerator.AppClasses
{
    public class TokenProviderOptions
    {   
        public string Issuer { get; set; }
 
        public string Audience { get; set; }
 
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1);
 
        public SigningCredentials SigningCredentials { get; set; }
    }

    public class TokenProvider
    {
        // secretKey contains a secret passphrase only your server knows
        private const string _secretKey = "as71ds94de8asd76ad7945c63ce";

        private byte[] keyBytes = Encoding.UTF8.GetBytes(_secretKey);

        private const string _audience = "localhost/*";

        internal SecurityToken _ValidToken { get; set; }
        internal SymmetricSecurityKey SigningKey { get; } =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
                
        internal TokenValidationParameters ValidationParameters 
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
                    ValidIssuer = "QuizOMaticIssuer",
                
                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = "QuizOMaticAudience",
                
                    // Validate the token expiry
                    ValidateLifetime = true,
                
                    // If you want to allow a certain amount of clock drift, set that here:
                    ClockSkew = TimeSpan.Zero
                };
            }
        }

        private readonly TokenProviderOptions _options;

        public TokenProvider() {
            _options = new TokenProviderOptions{
                Issuer = "QuizOMaticIssuer",
                Audience = "QuizOMaticAudience",
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(keyBytes), 
                        Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
        }

        public bool ValidateToken(string tokenString) 
        {
            JwtSecurityToken tokenReceived = new JwtSecurityToken(tokenString);

            
            if (keyBytes.Length < 64 && tokenReceived.SignatureAlgorithm == "HS256")
            {
                Array.Resize(ref keyBytes, 64);
            }
            
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validToken = null;
            try 
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(tokenString, ValidationParameters, out validToken);                
                _ValidToken = validToken;
                return claimsPrincipal.Identity.IsAuthenticated && validToken != null;    
            
            }
            catch (Exception ex) 
            {
                return false;            
            }        
        }

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
            catch (Exception ex) 
            {
                throw ex;
            }
           
            return encodedJwt;
        }

            /// <summary>
        /// Create JWT option
        /// </summary>
        /// <returns></returns>
        public static JwtBearerOptions CreateJwtBearerOption(RsaSecurityKey key, string tokenIssuer, string tokenAudience)
        {
            var jwtBearerOptions = new JwtBearerOptions();


            jwtBearerOptions.AutomaticAuthenticate = true;
            jwtBearerOptions.AutomaticChallenge = true;
            jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
            jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = key;
            jwtBearerOptions.TokenValidationParameters.ValidIssuer = tokenIssuer;
            jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
            jwtBearerOptions.TokenValidationParameters.ValidateLifetime = true;
            jwtBearerOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;



            jwtBearerOptions.TokenValidationParameters.ValidAudience = tokenAudience;
            return jwtBearerOptions;
        }
    }
}