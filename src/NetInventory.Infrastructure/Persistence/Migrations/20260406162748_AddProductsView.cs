using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_Products AS
                SELECT Id, Name, SKU, Category, QuantityInStock, UnitPrice, CreatedAt
                FROM Products
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_Products");
        }
    }
}
