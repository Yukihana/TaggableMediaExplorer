using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace TTX.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Descriptions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UUID = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TagsString = table.Column<string>(type: "TEXT", nullable: false),
                    Added = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descriptions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Hashes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GUID = table.Column<byte[]>(type: "BLOB", nullable: true),
                    SHA2 = table.Column<byte[]>(type: "BLOB", nullable: true),
                    FileCrumbs = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GUID = table.Column<byte[]>(type: "BLOB", nullable: true),
                    LastLocation = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MediaInfos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GUID = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    Container = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultVideoTrackCodec = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultAudioTrackCodec = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultSubtitlesFormat = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaInfos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Metadatas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GUID = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SizeBytes = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadatas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowAssign = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Descriptions");

            migrationBuilder.DropTable(
                name: "Hashes");

            migrationBuilder.DropTable(
                name: "Identities");

            migrationBuilder.DropTable(
                name: "MediaInfos");

            migrationBuilder.DropTable(
                name: "Metadatas");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}