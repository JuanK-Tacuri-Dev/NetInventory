namespace NetInventory.Infrastructure;

public static class Constants
{
    public static class Cache
    {
        public const string AuditConfigs         = "audit_configs";
        public const string GeneralValuesPrefix  = "general_values_";

        public static readonly TimeSpan AuditConfigsTtl = TimeSpan.FromMinutes(5);
    }
}
