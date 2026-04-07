using NetInventory.Resources;
using System.Net;
using static NetInventory.Domain.Constants.ErrorCodes;

namespace NetInventory.Domain.Common;

public sealed record Error(string Code, string Message, HttpStatusCode HttpStatus = HttpStatusCode.BadRequest)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static class General
    {
        public static readonly Error NotFound = new(
            Constants.ErrorCodes.General.NotFound,
            Messages.Error_NotFound,
            HttpStatusCode.NotFound);

        public static readonly Error InternalError = new(
            Constants.ErrorCodes.General.InternalError,
            Messages.Error_Internal,
            HttpStatusCode.InternalServerError);
    }

    public static class Product
    {
        public static readonly Error NotFound = new(
            Constants.ErrorCodes.Product.NotFound,
            Messages.Error_ProductNotFound,
            HttpStatusCode.NotFound);

        public static readonly Error SkuDuplicated = new(
            Constants.ErrorCodes.Product.SkuDuplicated,
            Messages.Error_SkuDuplicated);
    }

    public static class Sku
    {
        public static readonly Error Empty = new(
            Constants.ErrorCodes.Sku.Empty,
            Messages.Error_SkuEmpty);

        public static readonly Error TooLong = new(
            Constants.ErrorCodes.Sku.TooLong,
            Messages.Error_SkuTooLong);
    }

    public static class Money
    {
        public static readonly Error Negative = new(
            Constants.ErrorCodes.Money.Negative,
            Messages.Error_MoneyNegative);
    }

    public static class Stock
    {
        public static readonly Error Negative = new(
            Constants.ErrorCodes.Stock.Negative,
            Messages.Error_StockNegative);

        public static readonly Error InvalidQuantity = new(
            Constants.ErrorCodes.Stock.InvalidQuantity,
            Messages.Error_InvalidQuantity);

        public static readonly Error InvalidMovementType = new(
            Constants.ErrorCodes.Stock.InvalidMovementType,
            Messages.Error_InvalidMovementType);

        public static readonly Error StrategyNotFound = new(
            Constants.ErrorCodes.Stock.StrategyNotFound,
            Messages.Error_StrategyNotFound);
    }

    public static class Auth
    {
        public static readonly Error InvalidCredentials = new(
            Constants.ErrorCodes.Auth.InvalidCredentials,
            Messages.Error_InvalidCredentials,
            HttpStatusCode.Unauthorized);

        public static readonly Error InvalidRefreshToken = new(
            Constants.ErrorCodes.Auth.InvalidRefreshToken,
            Messages.Error_InvalidRefreshToken,
            HttpStatusCode.Unauthorized);

        public static readonly Error UserNotFound = new(
            Constants.ErrorCodes.Auth.UserNotFound,
            Messages.Error_UserNotFound,
            HttpStatusCode.NotFound);

        public static Error RegistrationFailed(string detail) =>
            new(Constants.ErrorCodes.Auth.RegistrationFailed, $"{Messages.Error_RegistrationFailed} {detail}");
    }

    public static class AuditConfig
    {
        public static readonly Error NotFound = new(
            Constants.ErrorCodes.AuditConfig.NotFound,
            Messages.Error_AuditConfigNotFound,
            HttpStatusCode.NotFound);
    }
}
