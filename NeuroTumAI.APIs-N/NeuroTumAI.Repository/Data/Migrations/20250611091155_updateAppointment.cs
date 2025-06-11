using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeuroTumAI.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_TicketId",
                table: "Appointment",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Ticket_TicketId",
                table: "Appointment",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Ticket_TicketId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_TicketId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Appointment");
        }
    }
}
