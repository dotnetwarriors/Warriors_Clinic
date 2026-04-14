using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warriors_Clinic.Migrations
{
    /// <inheritdoc />
    public partial class AddUserApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chemists",
                columns: table => new
                {
                    ChemistID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chemists", x => x.ChemistID);
                });

            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    DrugID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tittle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Expiry = table.Column<DateOnly>(type: "date", nullable: false),
                    Dosage = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.DrugID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "Physicians",
                columns: table => new
                {
                    PhysicianID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Specialization = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Physicians", x => x.PhysicianID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    RefrenceToID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    PhysicianId = table.Column<int>(type: "int", nullable: true),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Criticality = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ScheduleStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appointm__8ECDFCC204B77562", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK__Appointme__Patie__5812160E",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID");
                    table.ForeignKey(
                        name: "FK__Appointme__Physi__59063A47",
                        column: x => x.PhysicianId,
                        principalTable: "Physicians",
                        principalColumn: "PhysicianID");
                });

            migrationBuilder.CreateTable(
                name: "DrugRequests",
                columns: table => new
                {
                    DrugRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhysicianId = table.Column<int>(type: "int", nullable: true),
                    DrugInfoText = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    RequestStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DrugRequ__AEE9D630B65886DB", x => x.DrugRequestId);
                    table.ForeignKey(
                        name: "FK__DrugReque__Physi__66603565",
                        column: x => x.PhysicianId,
                        principalTable: "Physicians",
                        principalColumn: "PhysicianID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderHeader",
                columns: table => new
                {
                    Poid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PODate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    SupplierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__5F02A2D45CDE26B8", x => x.Poid);
                    table.ForeignKey(
                        name: "FK__PurchaseO__Suppl__6A30C649",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    ReceiverId = table.Column<int>(type: "int", nullable: true),
                    MessageText = table.Column<string>(type: "text", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Messages__C87C0C9CDDBF30F9", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK__Messages__Receiv__72C60C4A",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__Messages__Sender__71D1E811",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: true),
                    ScheduleDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ScheduleStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__9C8A5B495BE3D830", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK__Schedule__Appoin__5BE2A6F2",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLine",
                columns: table => new
                {
                    POLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Poid = table.Column<int>(type: "int", nullable: true),
                    DrugId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__07B9D3621DB03421", x => x.POLineId);
                    table.ForeignKey(
                        name: "FK__PurchaseO__DrugI__6E01572D",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "DrugID");
                    table.ForeignKey(
                        name: "FK__PurchaseOr__Poid__6D0D32F4",
                        column: x => x.Poid,
                        principalTable: "PurchaseOrderHeader",
                        principalColumn: "Poid");
                });

            migrationBuilder.CreateTable(
                name: "PhysicianAdvice",
                columns: table => new
                {
                    PhysicianAdviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: true),
                    Advice = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Physicia__82C625F0D3DA4DB3", x => x.PhysicianAdviceId);
                    table.ForeignKey(
                        name: "FK__Physician__Sched__5EBF139D",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId");
                });

            migrationBuilder.CreateTable(
                name: "PhysicianPrescription",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhysicianAdviceId = table.Column<int>(type: "int", nullable: true),
                    DrugId = table.Column<int>(type: "int", nullable: true),
                    Prescription = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Physicia__40130832B582FABF", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK__Physician__DrugI__628FA481",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "DrugID");
                    table.ForeignKey(
                        name: "FK__Physician__Physi__619B8048",
                        column: x => x.PhysicianAdviceId,
                        principalTable: "PhysicianAdvice",
                        principalColumn: "PhysicianAdviceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PhysicianId",
                table: "Appointments",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugRequests_PhysicianId",
                table: "DrugRequests",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicianAdvice_ScheduleId",
                table: "PhysicianAdvice",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicianPrescription_DrugId",
                table: "PhysicianPrescription",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicianPrescription_PhysicianAdviceId",
                table: "PhysicianPrescription",
                column: "PhysicianAdviceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeader_SupplierId",
                table: "PurchaseOrderHeader",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_DrugId",
                table: "PurchaseOrderLine",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_Poid",
                table: "PurchaseOrderLine",
                column: "Poid");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_AppointmentId",
                table: "Schedule",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chemists");

            migrationBuilder.DropTable(
                name: "DrugRequests");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PhysicianPrescription");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLine");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PhysicianAdvice");

            migrationBuilder.DropTable(
                name: "Drugs");

            migrationBuilder.DropTable(
                name: "PurchaseOrderHeader");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Physicians");
        }
    }
}
