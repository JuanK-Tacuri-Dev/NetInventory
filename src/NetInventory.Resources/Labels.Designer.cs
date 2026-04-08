using System.Globalization;
using System.Resources;

namespace NetInventory.Resources;

public static class Labels
{
    private static readonly ResourceManager _resourceManager =
        new("NetInventory.Resources.Labels", typeof(Labels).Assembly);

    private static CultureInfo _culture = CultureInfo.CurrentUICulture;

    public static CultureInfo Culture
    {
        get => _culture;
        set => _culture = value;
    }

    private static string Get(string name) =>
        _resourceManager.GetString(name, _culture) ?? name;

    // Acciones comunes
    public static string Action_Save           => Get(nameof(Action_Save));
    public static string Action_Saving         => Get(nameof(Action_Saving));
    public static string Action_Cancel         => Get(nameof(Action_Cancel));
    public static string Action_Back           => Get(nameof(Action_Back));
    public static string Action_Delete         => Get(nameof(Action_Delete));
    public static string Action_Edit           => Get(nameof(Action_Edit));
    public static string Action_Create         => Get(nameof(Action_Create));
    public static string Action_ViewDetail     => Get(nameof(Action_ViewDetail));
    public static string Action_RegisterMovement => Get(nameof(Action_RegisterMovement));

    // Estados
    public static string State_Loading         => Get(nameof(State_Loading));
    public static string State_LoadingRecords  => Get(nameof(State_LoadingRecords));
    public static string State_NoData          => Get(nameof(State_NoData));

    // Productos
    public static string Product_New           => Get(nameof(Product_New));
    public static string Product_Edit          => Get(nameof(Product_Edit));
    public static string Product_NewTitle      => Get(nameof(Product_NewTitle));
    public static string Product_EditTitle     => Get(nameof(Product_EditTitle));
    public static string Product_NewSubtitle   => Get(nameof(Product_NewSubtitle));
    public static string Product_EditSubtitle  => Get(nameof(Product_EditSubtitle));
    public static string Product_DeleteConfirm => Get(nameof(Product_DeleteConfirm));
    public static string Product_DeleteConfirmMsg => Get(nameof(Product_DeleteConfirmMsg));
    public static string Product_DeletedOk     => Get(nameof(Product_DeletedOk));
    public static string Product_CreatedOk     => Get(nameof(Product_CreatedOk));
    public static string Product_UpdatedOk     => Get(nameof(Product_UpdatedOk));
    public static string Product_CreateError   => Get(nameof(Product_CreateError));
    public static string Product_UpdateError   => Get(nameof(Product_UpdateError));
    public static string Product_SkuImmutable  => Get(nameof(Product_SkuImmutable));
    public static string Product_NoResults     => Get(nameof(Product_NoResults));
    public static string Product_EmptyList     => Get(nameof(Product_EmptyList));

    // Movimientos
    public static string Movement_Register     => Get(nameof(Movement_Register));
    public static string Movement_RegisterFor  => Get(nameof(Movement_RegisterFor));
    public static string Movement_RegisterError => Get(nameof(Movement_RegisterError));
    public static string Movement_RegisteredOk => Get(nameof(Movement_RegisteredOk));
    public static string Movement_NoHistory    => Get(nameof(Movement_NoHistory));

    // Configuración de Auditoría
    public static string AuditConfig_New       => Get(nameof(AuditConfig_New));
    public static string AuditConfig_Edit      => Get(nameof(AuditConfig_Edit));
    public static string AuditConfig_DeleteConfirm => Get(nameof(AuditConfig_DeleteConfirm));
    public static string AuditConfig_DeletedOk => Get(nameof(AuditConfig_DeletedOk));
    public static string AuditConfig_SaveError => Get(nameof(AuditConfig_SaveError));
    public static string AuditConfig_Empty     => Get(nameof(AuditConfig_Empty));
    public static string AuditConfig_CreatedOk => Get(nameof(AuditConfig_CreatedOk));
    public static string AuditConfig_UpdatedOk  => Get(nameof(AuditConfig_UpdatedOk));
    public static string AuditConfig_DeleteError => Get(nameof(AuditConfig_DeleteError));
    public static string AuditConfig_ToggleError => Get(nameof(AuditConfig_ToggleError));

    // Productos: acciones adicionales
    public static string Product_DeleteError     => Get(nameof(Product_DeleteError));

    // Autenticación
    public static string Auth_LoginTitle       => Get(nameof(Auth_LoginTitle));
    public static string Auth_RegisterTitle    => Get(nameof(Auth_RegisterTitle));
    public static string Auth_LoginButton      => Get(nameof(Auth_LoginButton));
    public static string Auth_RegisterButton   => Get(nameof(Auth_RegisterButton));
    public static string Auth_LogoutButton     => Get(nameof(Auth_LogoutButton));
    public static string Auth_LoginError       => Get(nameof(Auth_LoginError));
    public static string Auth_RegisterError    => Get(nameof(Auth_RegisterError));

    // Confirmaciones
    public static string Confirm_DeleteButton  => Get(nameof(Confirm_DeleteButton));
    public static string Confirm_Saved         => Get(nameof(Confirm_Saved));
    public static string Confirm_Deleted       => Get(nameof(Confirm_Deleted));
    public static string Confirm_Error         => Get(nameof(Confirm_Error));
    public static string Confirm_Updated       => Get(nameof(Confirm_Updated));
}
