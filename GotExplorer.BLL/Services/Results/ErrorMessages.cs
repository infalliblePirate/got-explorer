namespace GotExplorer.BLL.Services.Results
{
    public class ErrorMessages
    {
        public static readonly string UserServiceIncorrectUsername = "Username is incorrect";
        public static readonly string UserServiceIncorrectPassword = "Password is incorrect";
        public static readonly string UserServiceUserNotFound = "User not found";

        public static readonly string ImageServiceImageNotFound = "Image not found";
        public static readonly string ImageServiceFailedToUploadTheImage = "Failed to upload the image";
        public static readonly string ImageServiceFailedToUpdateTheImage = "Failed to update the image";
        public static readonly string ImageServiceFailedToDeleteTheImage = "Failed to delete the image";

        public static readonly string ModelServiceModelNotFound = "Model not found.";
        public static readonly string ModelServiceFailedToUploadTheModel = "Failed to upload the model.";
        public static readonly string ModelServiceFailedToUpdateTheModel = "Failed to update the model.";
        public static readonly string ModelServiceFailedToDeleteTheModel = "Failed to delete the model.";

        public static readonly string LevelServiceLevelNotFound = "Level not found";
        public static readonly string LevelServiceFailedToCreateLevel = "Failed to create the level";
        public static readonly string LevelServiceFailedToUpdateLevel = "Failed to update the level";
        public static readonly string LevelServiceFailedToDeleteLevel = "Failed to delete the level";

        public static readonly string EmailRequired = "Email is required.";
        public static readonly string InvalidEmail = "Email must be a valid email address.";
        public static readonly string PasswordRequired = "Password is required.";
        public static readonly string PasswordMinLength = "Password must be at least 8 characters long.";
        public static readonly string PasswordMustContainDigit = "Password must contain at least one digit.";
        public static readonly string PasswordMustContainLowercase = "Password must contain at least one lowercase letter.";
        public static readonly string PasswordMustContainUppercase = "Password must contain at least one uppercase letter.";
        public static readonly string PasswordMustContainSpecial = "Password must contain at least one special character.";
        public static readonly string UsernameRequired = "Username is required.";

        public static readonly string TokenRequired = "Token is required.";
        public static readonly string IdRequired = "Id is required.";

        public static readonly string UpdateUserImageId = "ImageId must be null or greater than 0.";

        public const string FileCannotBeEmpty = "File cannot be empty.";
        public const string FileSizeShouldBeLessThan = "File size should be less than {0} MB.";
        public const string InvalidImage = "Image is invalid.";
        public const string InvalidModel = "Model is invalid.";
    }
}
