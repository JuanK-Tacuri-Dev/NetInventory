namespace NetInventory.Client;

public static class Constants
{
    public static class LocalStorage
    {
        public const string TokenKey        = "auth_token";
        public const string ExpiresAtKey    = "auth_expires_at";
        public const string RefreshTokenKey = "auth_refresh_token";
    }

    public static class Api
    {
        public const string Login    = "/api/auth/login";
        public const string Register = "/api/auth/register";
        public const string Refresh  = "/api/auth/refresh";

        public const string Products      = "/api/products";
        public const string ProductsPaged = "/api/products/paged";

        public const string AuditConfigs      = "/api/audit-configs";
        public const string AuditConfigsCache = "/api/audit-configs/cache";

        public const string AuditLogs  = "/api/audit-logs";
        public const string ErrorLogs  = "/api/error-logs";

        public const string DiagnosticsError = "/api/diagnostics/error";

        public const string GeneralValues = "/api/general-values";
    }

    public static class Auth
    {
        public static readonly TimeSpan RefreshThreshold = TimeSpan.FromMinutes(5);
    }

    public static class GeneralTables
    {
        public const int CategoryTableId = 1001;
    }
}
