using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeuroTumAI.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateTicketLabDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LabDate",
                table: "Ticket",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabDate",
                table: "Ticket");
        }
    }
}
