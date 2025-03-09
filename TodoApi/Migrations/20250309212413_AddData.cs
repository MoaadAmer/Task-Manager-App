using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Controller_based_APIs.Migrations
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "DueDate", "IsComplete", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 3, 11, 23, 24, 12, 39, DateTimeKind.Local).AddTicks(8493), false, "Item 1", new DateTime(2025, 3, 10, 23, 24, 12, 39, DateTimeKind.Local).AddTicks(8299) },
                    { 2L, new DateTime(2025, 3, 12, 23, 24, 12, 39, DateTimeKind.Local).AddTicks(8505), false, "Item 2", new DateTime(2025, 3, 11, 23, 24, 12, 39, DateTimeKind.Local).AddTicks(8501) },
                    { 3L, new DateTime(2025, 3, 13, 23, 24, 12, 39, DateTimeKind.Local).AddTicks(8515), false, "Item 3", new DateTime(2025, 3, 12, 23, 24, 12, 39, DateTimeKind.Local).AddTicks(8511) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
