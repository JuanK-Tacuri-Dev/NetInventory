namespace NetInventory.Domain;

public static class Constants
{
    public static class ErrorCodes
    {
        public static class General
        {
            public const string NotFound      = "NOT_FOUND";
            public const string InternalError = "INTERNAL_ERROR";
        }

        public static class Product
        {
            public const string NotFound      = "PRODUCT_NOT_FOUND";
            public const string SkuDuplicated = "SKU_DUPLICATED";
        }

        public static class Sku
        {
            public const string Empty   = "SKU_EMPTY";
            public const string TooLong = "SKU_TOO_LONG";
        }

        public static class Money
        {
            public const string Negative = "MONEY_NEGATIVE";
        }

        public static class Stock
        {
            public const string Negative            = "STOCK_NEGATIVE";
            public const string InvalidQuantity     = "INVALID_QUANTITY";
            public const string InvalidMovementType = "INVALID_MOVEMENT_TYPE";
            public const string StrategyNotFound    = "STRATEGY_NOT_FOUND";
        }

        public static class Auth
        {
            public const string InvalidCredentials  = "INVALID_CREDENTIALS";
            public const string InvalidRefreshToken = "INVALID_REFRESH_TOKEN";
            public const string UserNotFound        = "USER_NOT_FOUND";
            public const string RegistrationFailed  = "REGISTRATION_FAILED";
        }

        public static class AuditConfig
        {
            public const string NotFound = "AUDIT_CONFIG_NOT_FOUND";
        }
    }
}
