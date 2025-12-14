using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultantsSalary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdinConsultantTaskAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ConsultantTaskAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ConsultantTaskAssignments");
        }
    }
}
