using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndAPI.Migrations
{
    public partial class AddedReturnRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ReturnRequest",
                columns: new[] { "Id", "AcceptedByUserId", "AssignmentId", "RequestedByUserId", "ReturnedDate", "State" },
                values: new object[] { 1, null, 1, 1, null, 0 });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3476ef8c-8fde-4099-a098-02ffe15f2c8e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "75f42109-818c-4aac-ae44-ec876285c194");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "516e72be-2360-4e58-ab12-66e71a578700");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "12df2bbe-bee4-46a8-aed3-6895922075c0");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "19f0f8be-c380-4a3a-9ddd-242c37799144");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "49cd27a3-c6d6-4861-8955-3b7428f14f7f");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReturnRequest",
                keyColumn: "Id",
                keyValue: 1);

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
    }
}
