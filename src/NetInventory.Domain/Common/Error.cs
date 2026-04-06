namespace NetInventory.Domain.Common;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error StockNegative = new(
        "STOCK_NEGATIVE",
        "La cantidad solicitada supera el stock disponible.");

    public static readonly Error ProductNotFound = new(
        "PRODUCT_NOT_FOUND",
        "El producto no existe.");

    public static readonly Error SkuDuplicated = new(
        "SKU_DUPLICATED",
        "Ya existe un producto con ese SKU.");

    public static readonly Error InvalidQuantity = new(
        "INVALID_QUANTITY",
        "La cantidad debe ser mayor a cero.");
}
