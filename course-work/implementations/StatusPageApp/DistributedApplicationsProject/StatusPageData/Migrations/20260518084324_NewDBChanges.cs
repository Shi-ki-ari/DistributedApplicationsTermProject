using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatusPageData.Migrations
{
    /// <inheritdoc />
    public partial class NewDBChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemGenerated",
                table: "IncidentsUpdates");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedEngineerId",
                table: "IncidentsEntities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemGenerated",
                table: "IncidentsEntities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "IncidentsEntities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[] { 1, "password123", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsSystemGenerated",
                table: "IncidentsEntities");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "IncidentsEntities");

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemGenerated",
                table: "IncidentsUpdates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "AssignedEngineerId",
                table: "IncidentsEntities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
