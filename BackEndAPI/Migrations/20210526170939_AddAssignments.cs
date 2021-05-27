using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndAPI.Migrations
{
    public partial class AddAssignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "State",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4,
                column: "State",
                value: 1);

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssetId", "AssignedByUserId", "AssignedDate", "AssignedToUserId", "Note", "State" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Make sure to upgrade RAM when you have spare time. Thanks.", 1 },
                    { 2, 2, 1, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Make sure to upgrade RAM when you have spare time. Thanks.", 0 },
                    { 3, 3, 4, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Make sure to upgrade RAM when you have spare time. Thanks.", 0 },
                    { 4, 4, 4, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Make sure to upgrade RAM when you have spare time. Thanks.", 1 }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "95fddc18-a05b-497e-b4a4-18e9d92e4e96");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "4271a09d-0946-450e-8d9e-92be52267655");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "c0866370-5976-4bd3-bed6-cc5319f1acec");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "234a9c22-c0a0-4d6f-a0d3-53ec0eac6fbc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "15bc8202-e0a6-471e-bd11-b7a31d40ea3b");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "932f1999-2b88-4916-96f2-4f358b15dc1e");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1,
                column: "State",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4,
                column: "State",
                value: 0);

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
    }
}
