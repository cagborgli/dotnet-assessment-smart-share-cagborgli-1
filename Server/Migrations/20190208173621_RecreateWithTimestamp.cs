using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Server.Migrations
{
    public partial class RecreateWithTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "filemanager",
                columns: table => new
                {
                    file_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    file_name = table.Column<string>(nullable: false),
                    file = table.Column<byte[]>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    time_created = table.Column<DateTime>(nullable: false),
                    expiration = table.Column<DateTime>(nullable: false),
                    max_download = table.Column<int>(nullable: false),
                    total_download = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filemanager", x => x.file_id);
                    table.UniqueConstraint("AlternateKey_FileName", x => x.file_name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filemanager");
        }
    }
}
