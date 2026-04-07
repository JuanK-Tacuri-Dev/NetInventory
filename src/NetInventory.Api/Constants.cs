namespace NetInventory.Api;

public static class Constants
{
    public static class Cors
    {
        public const string Dev  = "Dev";
        public const string Prod = "Prod";
    }

    public static class Headers
    {
        public const string CorrelationId = "X-Correlation-ID";
    }

    public static class Context
    {
        public const string CorrelationId = "CorrelationId";
    }

    public static class Audit
    {
        public const int MaxBodyLength = 8000;
    }
}
