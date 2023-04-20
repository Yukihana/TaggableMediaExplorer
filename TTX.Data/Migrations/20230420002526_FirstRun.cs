using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "Assets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<byte[]>(type: "BLOB", nullable: false),
                    LocalPath = table.Column<string>(type: "TEXT", nullable: false),
                    SizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Crumbs = table.Column<byte[]>(type: "BLOB", nullable: false),
                    VerifiedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SHA256 = table.Column<byte[]>(type: "BLOB", nullable: false),
                    MediaFormat = table.Column<string>(type: "TEXT", nullable: false),
                    MediaDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    PrimaryVideoCodec = table.Column<string>(type: "TEXT", nullable: false),
                    PrimaryVideoWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    PrimaryVideoHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    PrimaryVideoBitRate = table.Column<long>(type: "INTEGER", nullable: false),
                    PrimaryAudioCodec = table.Column<string>(type: "TEXT", nullable: false),
                    PrimaryAudioBitRate = table.Column<long>(type: "INTEGER", nullable: false),
                    PrimarySubtitleCodec = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TagsString = table.Column<string>(type: "TEXT", nullable: false),
                    AddedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SimilarIgnore = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    VectorIcon = table.Column<string>(type: "TEXT", nullable: false),
                    Color0 = table.Column<string>(type: "TEXT", nullable: false),
                    Color1 = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
