using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;

namespace NetInventory.Infrastructure.Persistence.ReadModel;

public sealed class ProductListRepository(IDbConnection connection) : IProductListRepository
{
    public async Task<PagedResult<ProductListItem>> GetPagedAsync(
        string ownerId,
        string? searchName,
        string? searchSku,
        string? searchCategory,
        string? searchStock,
        string? searchPrice,
        string[] categoryCodes,
        bool lowStockOnly,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var offset = (page - 1) * pageSize;
        var isSqlite = connection is SqliteConnection;

        var conditions = new List<string> { "OwnerId = @ownerId" };
        var parameters = new DynamicParameters();
        parameters.Add("ownerId", ownerId);

        if (!string.IsNullOrWhiteSpace(searchName))
        {
            conditions.Add("Name LIKE @searchName");
            parameters.Add("searchName", $"%{searchName}%");
        }

        if (!string.IsNullOrWhiteSpace(searchSku))
        {
            conditions.Add("SKU LIKE @searchSku");
            parameters.Add("searchSku", $"%{searchSku}%");
        }

        if (!string.IsNullOrWhiteSpace(searchCategory))
        {
            conditions.Add("CategoryDescription LIKE @searchCategory");
            parameters.Add("searchCategory", $"%{searchCategory}%");
        }

        if (!string.IsNullOrWhiteSpace(searchStock))
        {
            conditions.Add("CAST(QuantityInStock AS TEXT) LIKE @searchStock");
            parameters.Add("searchStock", $"%{searchStock}%");
        }

        if (!string.IsNullOrWhiteSpace(searchPrice))
        {
            conditions.Add("CAST(UnitPrice AS TEXT) LIKE @searchPrice");
            parameters.Add("searchPrice", $"%{searchPrice}%");
        }

        if (categoryCodes.Length > 0)
        {
            conditions.Add("CategoryCode IN @categoryCodes");
            parameters.Add("categoryCodes", categoryCodes);
        }

        if (lowStockOnly)
            conditions.Add("QuantityInStock < MinStock");

        var where = "WHERE " + string.Join(" AND ", conditions);

        var countSql = $"SELECT COUNT(*) FROM vw_Products {where}";

        parameters.Add("pageSize", pageSize);
        parameters.Add("offset", offset);

        var dataSql = isSqlite
            ? $"SELECT * FROM vw_Products {where} ORDER BY Name LIMIT @pageSize OFFSET @offset"
            : $"SELECT * FROM vw_Products {where} ORDER BY Name OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

        var totalCount = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: ct));
        var items = await connection.QueryAsync<ProductListItem>(new CommandDefinition(dataSql, parameters, cancellationToken: ct));

        return new PagedResult<ProductListItem>(items, totalCount, page, pageSize);
    }
}
