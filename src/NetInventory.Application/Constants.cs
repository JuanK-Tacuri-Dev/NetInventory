namespace NetInventory.Application;

public static class Constants
{
    public static class Cache
    {
        public const string GeneralValuesPrefix = "general_values_";
    }

    public static class GeneralTables
    {
        public const int CategoryTableId = 1001;
    }

    public static class Auth
    {
        public static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);
    }
}
