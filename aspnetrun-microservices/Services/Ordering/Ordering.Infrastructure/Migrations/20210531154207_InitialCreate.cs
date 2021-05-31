using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    State = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CardName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Expiration = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CVV = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
