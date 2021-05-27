using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndAPI.Migrations
{
    public partial class AddAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetCode", "AssetName", "CategoryId", "InstalledDate", "Location", "Specification", "State" },
                values: new object[,]
                {
                    { 1, "LA000001", "Laptop 1", 1, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Balls-to-the-walls laptop, specced with the latest CPU and GPU", 0 },
                    { 2, "LA000002", "Laptop 2", 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "An even more balls-to-the-walls laptop, specced with even better CPU and GPU than laptop 1", 0 },
                    { 3, "PC000001", "PC 1", 2, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Balls-to-the-walls desktop, designed for ultimate Word experience", 0 },
                    { 4, "PC000002", "PC 2", 2, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "An even more balls-to-the-walls laptop, designed for the performant Excel workflow", 0 }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d6fcb75b-4759-4375-8a17-dcc3ce7cf83a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "341017d0-3fe6-4b56-871a-c5a4ec32586f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "8ac0f348-8e15-4fc4-a3bd-373f57cd819e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "e353d97e-3b24-4056-a49d-241aed7f83d5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "d822ca0a-7bc9-46a6-90f9-3d4c42deba41");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "7d73b1aa-8ec7-4ba8-88bf-e37afbf4cf71");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6a80d73c-b992-4447-a7bd-dce727c616e0");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b284a8a7-4fb1-45b6-8aac-8d331524e800");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "24a08db8-c98d-4134-ae72-6c7405f2e0cc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "628abad7-fff2-4bcc-a47f-791082e57ce7");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "b97d1b0a-71f9-44dc-aa6d-e2a4ffb8af6c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "da99ec9d-6d27-4c1c-a7ef-183d4b7b78ea");
        }
    }
}
