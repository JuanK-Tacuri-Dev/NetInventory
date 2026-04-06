using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;

namespace NetInventory.Infrastructure.Persistence.ReadModel;

public sealed class ProductListRepository(IDbConnection connection) : IProductListRepository
{
    public async Task<PagedResult<ProductListItem>> GetPagedAsync(
        string? category,
        bool lowStockOnly,
        int threshold,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var offset = (page - 1) * pageSize;
        var isSqlite = connection is SqliteConnection;

        var conditions = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(category))
        {
            conditions.Add("Category = @category");
            parameters.Add("category", category);
        }

        if (lowStockOnly)
        {
            conditions.Add("QuantityInStock < @threshold");
            parameters.Add("threshold", threshold);
        }

        var where = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;

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
