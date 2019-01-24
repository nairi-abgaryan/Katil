using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Katil.Data.Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FileGuid = table.Column<Guid>(nullable: false),
                    DisputeGuid = table.Column<Guid>(nullable: false),
                    FileType = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    FileMimeType = table.Column<string>(maxLength: 255, nullable: false),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    FileDate = table.Column<DateTime>(nullable: true),
                    OriginalFileName = table.Column<string>(maxLength: 255, nullable: false),
                    FileSize = table.Column<int>(nullable: false),
                    FilePath = table.Column<string>(maxLength: 255, nullable: false),
                    FileTitle = table.Column<string>(maxLength: 100, nullable: true),
                    FileStatus = table.Column<byte>(nullable: true, defaultValue: (byte)0),
                    AddedBy = table.Column<int>(nullable: true),
                    FileConsidered = table.Column<bool>(nullable: true, defaultValue: true),
                    FileReferenced = table.Column<bool>(nullable: true, defaultValue: false),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    SystemSettingsId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.SystemSettingsId);
                });

            migrationBuilder.CreateTable(
                name: "SystemUserRoles",
                columns: table => new
                {
                    SystemUserRoleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false),
                    RoleDescritption = table.Column<string>(maxLength: 255, nullable: false),
                    SessionDuration = table.Column<int>(nullable: false, defaultValue: 900)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUserRoles", x => x.SystemUserRoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserTokenId = table.Column<int>(nullable: false),
                    AuthToken = table.Column<string>(maxLength: 250, nullable: false),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    ExpiresOn = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SystemUserGuid = table.Column<Guid>(nullable: true),
                    ParticipantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 250, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    AccountMobile = table.Column<string>(maxLength: 15, nullable: true),
                    SystemUserRoleId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_SystemUserRoles_SystemUserRoleId",
                        column: x => x.SystemUserRoleId,
                        principalTable: "SystemUserRoles",
                        principalColumn: "SystemUserRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SystemUserRoleId",
                table: "Users",
                column: "SystemUserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "SystemUserRoles");
        }
    }
}
