using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorMatch.Migrations
{
    /// <inheritdoc />
    public partial class AjusteNuevoPC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewCount",
                table: "Reviews",
                newName: "Rating");

            migrationBuilder.AddColumn<int>(
                name: "CancelCount",
                table: "TutorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "TutorProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyScore",
                table: "TutorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "TutorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TutorId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelCount",
                table: "TutorProfiles");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "TutorProfiles");

            migrationBuilder.DropColumn(
                name: "PenaltyScore",
                table: "TutorProfiles");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "TutorProfiles");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "TutorId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Reviews",
                newName: "ReviewCount");
        }
    }
}
