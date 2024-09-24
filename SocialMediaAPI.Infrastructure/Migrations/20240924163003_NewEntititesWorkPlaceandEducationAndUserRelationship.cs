using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewEntititesWorkPlaceandEducationAndUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Educations",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "RelationshipStatus",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "WorkPlaces",
                table: "Profiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ViewedAt",
                table: "StoryViewers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 81, DateTimeKind.Local).AddTicks(5647),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 243, DateTimeKind.Local).AddTicks(817));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Stories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 79, DateTimeKind.Local).AddTicks(8910),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 242, DateTimeKind.Local).AddTicks(5888));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 62, DateTimeKind.Local).AddTicks(3108),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 237, DateTimeKind.Local).AddTicks(3312));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 34, DateTimeKind.Local).AddTicks(2929),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 234, DateTimeKind.Local).AddTicks(2655));

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterId = table.Column<int>(type: "int", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRelationships_Profiles_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRelationships_Profiles_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlaces_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_ProfileId",
                table: "Educations",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_PartnerId",
                table: "UserRelationships",
                column: "PartnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_RequesterId",
                table: "UserRelationships",
                column: "RequesterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_ProfileId",
                table: "WorkPlaces",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "UserRelationships");

            migrationBuilder.DropTable(
                name: "WorkPlaces");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ViewedAt",
                table: "StoryViewers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 243, DateTimeKind.Local).AddTicks(817),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 81, DateTimeKind.Local).AddTicks(5647));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Stories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 242, DateTimeKind.Local).AddTicks(5888),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 79, DateTimeKind.Local).AddTicks(8910));

            migrationBuilder.AddColumn<string>(
                name: "Educations",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RelationshipStatus",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPlaces",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 237, DateTimeKind.Local).AddTicks(3312),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 62, DateTimeKind.Local).AddTicks(3108));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 18, 14, 28, 29, 234, DateTimeKind.Local).AddTicks(2655),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 24, 19, 30, 0, 34, DateTimeKind.Local).AddTicks(2929));
        }
    }
}
