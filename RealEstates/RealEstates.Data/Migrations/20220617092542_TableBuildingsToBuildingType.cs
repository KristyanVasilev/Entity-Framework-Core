using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstates.Data.Migrations
{
    public partial class TableBuildingsToBuildingType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Buildings_BuildingTypeId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Buildings",
                table: "Buildings");

            migrationBuilder.RenameTable(
                name: "Buildings",
                newName: "BuildingsType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuildingsType",
                table: "BuildingsType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_BuildingsType_BuildingTypeId",
                table: "Properties",
                column: "BuildingTypeId",
                principalTable: "BuildingsType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_BuildingsType_BuildingTypeId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingsType",
                table: "BuildingsType");

            migrationBuilder.RenameTable(
                name: "BuildingsType",
                newName: "Buildings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Buildings",
                table: "Buildings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Buildings_BuildingTypeId",
                table: "Properties",
                column: "BuildingTypeId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
