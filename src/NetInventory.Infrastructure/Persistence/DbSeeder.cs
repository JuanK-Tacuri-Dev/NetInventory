using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await SeedGeneralTablesAsync(db);
        await SeedGeneralValuesAsync(db);
    }

    private static async Task SeedGeneralTablesAsync(AppDbContext db)
    {
        if (await db.Set<GeneralTable>().AnyAsync()) return;

        db.Set<GeneralTable>().AddRange(
            GeneralTable.Create(1001, "Categorías de Producto"),
            GeneralTable.Create(1002, "Parámetros del Sistema")
        );

        await db.SaveChangesAsync();
    }

    private static async Task SeedGeneralValuesAsync(AppDbContext db)
    {
        if (await db.Set<GeneralValue>().AnyAsync()) return;

        db.Set<GeneralValue>().AddRange(
            // Categorías de producto
            GeneralValue.Create(1001, "ELEC",  "Electrónica",          sortOrder: 1),
            GeneralValue.Create(1001, "ROPA",  "Ropa y Accesorios",    sortOrder: 2),
            GeneralValue.Create(1001, "ALIM",  "Alimentos y Bebidas",  sortOrder: 3),
            GeneralValue.Create(1001, "OFIC",  "Oficina y Papelería",  sortOrder: 4),
            GeneralValue.Create(1001, "HERRA", "Herramientas",         sortOrder: 5),
            GeneralValue.Create(1001, "HOGAR", "Hogar y Jardín",       sortOrder: 6),
            GeneralValue.Create(1001, "SALUD", "Salud y Belleza",      sortOrder: 7),
            GeneralValue.Create(1001, "DEPO",  "Deportes",             sortOrder: 8),
            GeneralValue.Create(1001, "OTROS", "Otros",                sortOrder: 9),

            // Parámetros del sistema
            GeneralValue.Create(1002, "LOW_STOCK_MINUTES",       "Intervalo de verificación de stock bajo (minutos)",       sortOrder: 1, value: "4"),
            GeneralValue.Create(1002, "NOTIFICATION_POLL_MINUTES", "Intervalo de polling de notificaciones en el cliente (minutos)", sortOrder: 2, value: "5")
        );

        await db.SaveChangesAsync();
    }
}
