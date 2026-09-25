using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftoftheGiverz.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDonorsTaxCertificatesAndVolunteerAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    DonorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Organization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PreferredCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TotalDonated = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.DonorId);
                });

            migrationBuilder.CreateTable(
                name: "TaxCertificates",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationId = table.Column<int>(type: "int", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCertificates", x => x.CertificateId);
                    table.ForeignKey(
                        name: "FK_TaxCertificates_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "DonationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VolunteerId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerAssignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_VolunteerAssignments_ReliefProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ReliefProjects",
                        principalColumn: "ReliefProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerAssignments_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "VolunteerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxCertificates_DonationId",
                table: "TaxCertificates",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerAssignments_ProjectId",
                table: "VolunteerAssignments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerAssignments_VolunteerId",
                table: "VolunteerAssignments",
                column: "VolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropTable(
                name: "TaxCertificates");

            migrationBuilder.DropTable(
                name: "VolunteerAssignments");
        }
    }
}
