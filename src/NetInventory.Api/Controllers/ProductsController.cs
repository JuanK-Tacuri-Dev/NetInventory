using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common;
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
public sealed class ProductsController(
    GetProductsQueryHandler getProductsHandler,
    GetProductByIdQueryHandler getProductByIdHandler,
    GetProductsPagedQueryHandler getProductsPagedHandler,
    CreateProductCommandHandler createProductHandler,
    UpdateProductCommandHandler updateProductHandler,
    DeleteProductCommandHandler deleteProductHandler,
    RegisterMovementCommandHandler registerMovementHandler,
    GetMovementsQueryHandler getMovementsHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? category,
        [FromQuery] bool lowStockOnly = false,
        [FromQuery] int threshold = 10,
        CancellationToken ct = default)
    {
        var query = new GetProductsQuery(category, lowStockOnly, threshold);
        var result = await getProductsHandler.HandleAsync(query, ct);
        return Ok(ApiResponse<object>.Ok(result.Value));
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetProductsPaged(
        [FromQuery] string? category,
        [FromQuery] bool lowStockOnly = false,
        [FromQuery] int threshold = 10,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var query = new GetProductsPagedQuery(category, lowStockOnly, threshold, page, pageSize);
        var result = await getProductsPagedHandler.HandleAsync(query, ct);
        return Ok(ApiResponse<object>.Ok(result.Value));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await getProductByIdHandler.HandleAsync(new GetProductByIdQuery(id), ct);
        if (result.IsFailure)
            return result.Error.Code == "PRODUCT_NOT_FOUND"
                ? NotFound(ApiResponse.Fail(result.Error.Message, result.Error.Code))
                : BadRequest(ApiResponse.Fail(result.Error.Message, result.Error.Code));

        return Ok(ApiResponse<object>.Ok(result.Value));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken ct = default)
    {
        var result = await createProductHandler.HandleAsync(command, ct);
        if (result.IsFailure)
            return result.Error.Code == "PRODUCT_NOT_FOUND"
                ? NotFound(ApiResponse.Fail(result.Error.Message, result.Error.Code))
                : BadRequest(ApiResponse.Fail(result.Error.Message, result.Error.Code));

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value.Id },
            ApiResponse<object>.Ok(result.Value));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken ct = default)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Category, request.UnitPrice);
        var result = await updateProductHandler.HandleAsync(command, ct);
        if (result.IsFailure)
            return result.Error.Code == "PRODUCT_NOT_FOUND"
                ? NotFound(ApiResponse.Fail(result.Error.Message, result.Error.Code))
                : BadRequest(ApiResponse.Fail(result.Error.Message, result.Error.Code));

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var result = await deleteProductHandler.HandleAsync(new DeleteProductCommand(id), ct);
        if (result.IsFailure)
            return result.Error.Code == "PRODUCT_NOT_FOUND"
                ? NotFound(ApiResponse.Fail(result.Error.Message, result.Error.Code))
                : BadRequest(ApiResponse.Fail(result.Error.Message, result.Error.Code));

        return NoContent();
    }

    [HttpPost("{id:guid}/movements")]
    public async Task<IActionResult> RegisterMovement(Guid id, [FromBody] RegisterMovementRequest request, CancellationToken ct = default)
    {
        var command = new RegisterMovementCommand(id, request.Type, request.Quantity);
        var result = await registerMovementHandler.HandleAsync(command, ct);
        if (result.IsFailure)
            return result.Error.Code == "PRODUCT_NOT_FOUND"
                ? NotFound(ApiResponse.Fail(result.Error.Message, result.Error.Code))
                : BadRequest(ApiResponse.Fail(result.Error.Message, result.Error.Code));

        return CreatedAtAction(
            nameof(GetMovements),
            new { id },
            ApiResponse<object>.Ok(result.Value));
    }

    [HttpGet("{id:guid}/movements")]
    public async Task<IActionResult> GetMovements(Guid id, CancellationToken ct = default)
    {
        var result = await getMovementsHandler.HandleAsync(new GetMovementsQuery(id), ct);
        if (result.IsFailure)
            return result.Error.Code == "PRODUCT_NOT_FOUND"
                ? NotFound(ApiResponse.Fail(result.Error.Message, result.Error.Code))
                : BadRequest(ApiResponse.Fail(result.Error.Message, result.Error.Code));

        return Ok(ApiResponse<object>.Ok(result.Value));
    }
}

public sealed record UpdateProductRequest(string Name, string Category, decimal UnitPrice);
public sealed record RegisterMovementRequest(string Type, int Quantity);
