﻿namespace GotExplorer.BLL.Services.Results
{
    public static partial class ErrorCodes
    {
        public const string None = nameof(None);

        public const string NotFound = nameof(NotFound);
        public const string Unauthorized = nameof(Unauthorized);
        public const string Invalid = nameof(Invalid);
        public const string Forbidden = nameof(Forbidden);
        public const string UserCreationFailed = nameof(UserCreationFailed);
        public const string RoleAssignmentFailed = nameof(RoleAssignmentFailed);
    }
}
