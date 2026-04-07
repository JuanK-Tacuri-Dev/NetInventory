using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common;
using NetInventory.Api.Common.Extensions;
using NetInventory.Api.Requests.Products;
using NetInventory.Application.Common;
using NetInventory.Application.Products.Commands.CreateProduct;
using NetInventory.Application.Products.Commands.DeleteProduct;
using NetInventory.Application.Products.Commands.UpdateProduct;
using NetInventory.Application.Products.Queries.GetProductById;
using NetInventory.Application.Products.Queries.GetProducts;
using NetInventory.Application.Products.Queries.GetProductsPaged;
using NetInventory.Application.StockMovements.Commands.RegisterMovement;
using NetInventory.Application.StockMovements.Queries.GetMovements;

namespace NetInventory.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public sealed class ProductsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? categoryCode,
        [FromQuery] bool lowStockOnly = false,
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetProductsQuery(categoryCode, lowStockOnly), ct);

        return result.ToActionResult(this);
    }

    [HttpPost("paged")]
    public async Task<IActionResult> GetProductsPaged(
        [FromBody] GetProductsPagedRequest request,
        CancellationToken ct = default)
    {
        var query = new GetProductsPagedQuery(
            request.SearchName, request.SearchSku, request.SearchCategory,
            request.SearchStock, request.SearchPrice,
            request.CategoryCodes ?? [], request.LowStockOnly,
            request.Page, request.PageSize);

        var result = await dispatcher.QueryAsync(query, ct);

        return result.ToActionResult(this);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetProductByIdQuery(id), ct);

        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(
            new CreateProductCommand(
                request.Name, request.SKU, request.CategoryTableId,
                request.CategoryCode, request.UnitPrice, request.MinStock, request.MaxStock),
            ct);

        return result.ToActionResult(this,
            value => CreatedAtAction(nameof(GetById), new { id = value.Id }, ApiResponse<object>.Ok(value)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(
            new UpdateProductCommand(
                id, request.Name, request.CategoryTableId,
                request.CategoryCode, request.UnitPrice, request.MinStock, request.MaxStock),
            ct);

        return result.ToActionResult(this);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(new DeleteProductCommand(id), ct);

        return result.ToActionResult(this);
    }

    [HttpPost("{id:guid}/movements")]
    public async Task<IActionResult> RegisterMovement(
        Guid id,
        [FromBody] RegisterMovementRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(
            new RegisterMovementCommand(id, request.Type, request.Quantity, request.Reason), ct);

        return result.ToActionResult(this,
            value => CreatedAtAction(nameof(GetMovements), new { id }, ApiResponse<object>.Ok(value)));
    }

    [HttpGet("{id:guid}/movements")]
    public async Task<IActionResult> GetMovements(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetMovementsQuery(id), ct);

        return result.ToActionResult(this);
    }
}
