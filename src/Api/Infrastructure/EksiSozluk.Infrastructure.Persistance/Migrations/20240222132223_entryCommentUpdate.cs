using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EksiSozluk.Infrastructure.Persistance.Migrations
{
    public partial class entryCommentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_entrycomment_entry_CreatedById",
                schema: "dbo",
                table: "entrycomment");

            migrationBuilder.CreateIndex(
                name: "IX_entrycomment_EntryId",
                schema: "dbo",
                table: "entrycomment",
                column: "EntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_entrycomment_entry_EntryId",
                schema: "dbo",
                table: "entrycomment",
                column: "EntryId",
                principalSchema: "dbo",
                principalTable: "entry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_entrycomment_entry_EntryId",
                schema: "dbo",
                table: "entrycomment");

            migrationBuilder.DropIndex(
                name: "IX_entrycomment_EntryId",
                schema: "dbo",
                table: "entrycomment");

            migrationBuilder.AddForeignKey(
                name: "FK_entrycomment_entry_CreatedById",
                schema: "dbo",
                table: "entrycomment",
                column: "CreatedById",
                principalSchema: "dbo",
                principalTable: "entry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
