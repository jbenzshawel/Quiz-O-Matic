using Microsoft.AspNetCore.Identity;

namespace ApiQuizGenerator.Models.AccountViewModels
{
    public class AuthenticationResult
    {
        public SignInResult SignInResult { get; set; }

        public string AuthKey { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool RememberMe { get; set; }

        public bool LockOut { get; set; }
    }
}