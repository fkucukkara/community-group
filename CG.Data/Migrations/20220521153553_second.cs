using Microsoft.EntityFrameworkCore.Migrations;

namespace CG.Data.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_CommunityGroups_CommunityGroupId",
                table: "Persons");

            migrationBuilder.AlterColumn<int>(
                name: "CommunityGroupId",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_CommunityGroups_CommunityGroupId",
                table: "Persons",
                column: "CommunityGroupId",
                principalTable: "CommunityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_CommunityGroups_CommunityGroupId",
                table: "Persons");

            migrationBuilder.AlterColumn<int>(
                name: "CommunityGroupId",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_CommunityGroups_CommunityGroupId",
                table: "Persons",
                column: "CommunityGroupId",
                principalTable: "CommunityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
