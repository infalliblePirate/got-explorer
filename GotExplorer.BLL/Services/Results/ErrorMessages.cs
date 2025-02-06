namespace GotExplorer.BLL.Services.Results
{
    public class ErrorMessages
    {
        public static readonly string UserServiceIncorrectUsername = "Username is incorrect";
        public static readonly string UserServiceIncorrectPassword = "Password is incorrect";

        public static readonly string EmailRequired = "Email is required.";
        public static readonly string InvalidEmail = "Email must be a valid email address.";
        public static readonly string PasswordRequired = "Password is required.";
        public static readonly string PasswordMinLength = "Password must be at least 8 characters long.";
        public static readonly string PasswordMustContainDigit = "Password must contain at least one digit.";
        public static readonly string PasswordMustContainLowercase = "Password must contain at least one lowercase letter.";
        public static readonly string PasswordMustContainUppercase = "Password must contain at least one uppercase letter.";
        public static readonly string PasswordMustContainSpecial = "Password must contain at least one special character.";
        public static readonly string UsernameRequired = "Username is required.";
    }
}
