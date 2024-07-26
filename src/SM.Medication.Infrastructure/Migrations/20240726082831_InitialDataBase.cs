using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SM.Medication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Medication",
                schema: "dbo",
                columns: table => new
                {
                    MedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medication", x => x.MedicationId);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Medication",
                columns: new[] { "MedicationId", "CreatedAt", "CreatedBy", "ModifiedAt", "ModifiedBy", "Name", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 26, 8, 28, 30, 203, DateTimeKind.Utc).AddTicks(2807), "DbContext", new DateTime(2024, 7, 26, 8, 28, 30, 203, DateTimeKind.Utc).AddTicks(2809), "DbContex", "Paracetamol", 100 },
                    { 2, new DateTime(2024, 7, 26, 8, 28, 30, 203, DateTimeKind.Utc).AddTicks(2811), "DbContext", new DateTime(2024, 7, 26, 8, 28, 30, 203, DateTimeKind.Utc).AddTicks(2811), "DbContex", "Ibuprofen", 50 },
                    { 3, new DateTime(2024, 7, 26, 8, 28, 30, 203, DateTimeKind.Utc).AddTicks(2813), "DbContext", new DateTime(2024, 7, 26, 8, 28, 30, 203, DateTimeKind.Utc).AddTicks(2813), "DbContex", "Aspirin", 75 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medication",
                schema: "dbo");
        }
    }
}
