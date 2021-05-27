using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndAPI.Migrations
{
    public partial class AddUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "JoinedDate", "LastName", "Location", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StaffCode", "Status", "TwoFactorEnabled", "Type", "UserName" },
                values: new object[,]
                {
                    { 3, 0, "24a08db8-c98d-4134-ae72-6c7405f2e0cc", new DateTime(1997, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Binh", 1, new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Thi", 0, false, null, null, null, "binhnt2@12011997", null, null, false, null, "SD0003", 0, false, 1, "binhnt2" },
                    { 4, 0, "628abad7-fff2-4bcc-a47f-791082e57ce7", new DateTime(2000, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Anh", 1, new DateTime(2018, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Duc", 1, false, null, null, null, "anhnd@20012000", null, null, false, null, "SD0004", 0, false, 0, "anhnd" },
                    { 5, 0, "b97d1b0a-71f9-44dc-aa6d-e2a4ffb8af6c", new DateTime(1990, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Van", 1, new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Thi", 1, false, null, null, null, "binhnt@12011990", null, null, false, null, "SD0005", 0, false, 1, "binhnt" },
                    { 6, 0, "da99ec9d-6d27-4c1c-a7ef-183d4b7b78ea", new DateTime(1987, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Binh", 0, new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Thi", 1, false, null, null, null, "binhnt2@120187", null, null, false, null, "SD0006", 0, false, 1, "binhnt2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b531f734-17b3-42c5-b5fe-db9e6641cedb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "388c8fe9-1557-438b-952a-0dfbb1e545b1");
        }
    }
}
