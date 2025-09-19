using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaCitasMedicas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InicialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_especializacion",
                columns: table => new
                {
                    IdEspecializacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_especializacion", x => x.IdEspecializacion);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_rol", x => x.IdRol);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_t_usuario_t_rol_IdRol",
                        column: x => x.IdRol,
                        principalTable: "t_rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_doctor",
                columns: table => new
                {
                    IdDoctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Apellido = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CedulaProfesional = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Horario = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_doctor", x => x.IdDoctor);
                    table.ForeignKey(
                        name: "FK_t_doctor_t_usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "t_usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_paciente",
                columns: table => new
                {
                    IdPaciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seguro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_paciente", x => x.IdPaciente);
                    table.ForeignKey(
                        name: "FK_t_paciente_t_usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "t_usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_doctor_especializacion",
                columns: table => new
                {
                    IdDoctorEspecialidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdDoctor = table.Column<int>(type: "int", nullable: false),
                    IdEspecializacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_doctor_especializacion", x => x.IdDoctorEspecialidad);
                    table.ForeignKey(
                        name: "FK_t_doctor_especializacion_t_doctor_IdDoctor",
                        column: x => x.IdDoctor,
                        principalTable: "t_doctor",
                        principalColumn: "IdDoctor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_doctor_especializacion_t_especializacion_IdEspecializacion",
                        column: x => x.IdEspecializacion,
                        principalTable: "t_especializacion",
                        principalColumn: "IdEspecializacion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_cita",
                columns: table => new
                {
                    IdCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    IdDoctor = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_cita", x => x.IdCita);
                    table.ForeignKey(
                        name: "FK_t_cita_t_doctor_IdDoctor",
                        column: x => x.IdDoctor,
                        principalTable: "t_doctor",
                        principalColumn: "IdDoctor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_cita_t_paciente_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "t_paciente",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_cita_t_usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "t_usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_historial_medico",
                columns: table => new
                {
                    IdHistorialMedico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    Notas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Diagnostico = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tratamientos = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CuadroMedico = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_historial_medico", x => x.IdHistorialMedico);
                    table.ForeignKey(
                        name: "FK_t_historial_medico_t_paciente_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "t_paciente",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_t_cita_IdDoctor",
                table: "t_cita",
                column: "IdDoctor");

            migrationBuilder.CreateIndex(
                name: "IX_t_cita_IdPaciente",
                table: "t_cita",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_t_cita_IdUsuario",
                table: "t_cita",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_t_doctor_IdUsuario",
                table: "t_doctor",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_t_doctor_especializacion_IdDoctor",
                table: "t_doctor_especializacion",
                column: "IdDoctor");

            migrationBuilder.CreateIndex(
                name: "IX_t_doctor_especializacion_IdEspecializacion",
                table: "t_doctor_especializacion",
                column: "IdEspecializacion");

            migrationBuilder.CreateIndex(
                name: "IX_t_historial_medico_IdPaciente",
                table: "t_historial_medico",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_t_paciente_IdUsuario",
                table: "t_paciente",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_t_usuario_IdRol",
                table: "t_usuario",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_cita");

            migrationBuilder.DropTable(
                name: "t_doctor_especializacion");

            migrationBuilder.DropTable(
                name: "t_historial_medico");

            migrationBuilder.DropTable(
                name: "t_doctor");

            migrationBuilder.DropTable(
                name: "t_especializacion");

            migrationBuilder.DropTable(
                name: "t_paciente");

            migrationBuilder.DropTable(
                name: "t_usuario");

            migrationBuilder.DropTable(
                name: "t_rol");
        }
    }
}
