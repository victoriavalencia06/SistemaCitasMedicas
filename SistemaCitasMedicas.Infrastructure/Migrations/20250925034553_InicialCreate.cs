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
                    idespecializacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(75)", maxLength: 75, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_especializacion", x => x.idespecializacion);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_rol",
                columns: table => new
                {
                    idrol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(75)", maxLength: 75, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_rol", x => x.idrol);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_usuario",
                columns: table => new
                {
                    idusuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idrol = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    correo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_usuario", x => x.idusuario);
                    table.ForeignKey(
                        name: "FK_t_usuario_t_rol_idrol",
                        column: x => x.idrol,
                        principalTable: "t_rol",
                        principalColumn: "idrol",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_doctor",
                columns: table => new
                {
                    id_doctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idusuario = table.Column<long>(type: "bigint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cedulaprofesional = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    horario = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_doctor", x => x.id_doctor);
                    table.ForeignKey(
                        name: "FK_t_doctor_t_usuario_idusuario",
                        column: x => x.idusuario,
                        principalTable: "t_usuario",
                        principalColumn: "idusuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_paciente",
                columns: table => new
                {
                    idpaciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idusuario = table.Column<long>(type: "bigint", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fechanacimiento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    telefono = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    seguro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_paciente", x => x.idpaciente);
                    table.ForeignKey(
                        name: "FK_t_paciente_t_usuario_idusuario",
                        column: x => x.idusuario,
                        principalTable: "t_usuario",
                        principalColumn: "idusuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_doctor_especializacion",
                columns: table => new
                {
                    idoctorespecializacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_doctor = table.Column<int>(type: "int", nullable: false),
                    idespecializacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_doctor_especializacion", x => x.idoctorespecializacion);
                    table.ForeignKey(
                        name: "FK_t_doctor_especializacion_t_doctor_id_doctor",
                        column: x => x.id_doctor,
                        principalTable: "t_doctor",
                        principalColumn: "id_doctor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_doctor_especializacion_t_especializacion_idespecializacion",
                        column: x => x.idespecializacion,
                        principalTable: "t_especializacion",
                        principalColumn: "idespecializacion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_cita",
                columns: table => new
                {
                    idcita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idusuario = table.Column<long>(type: "bigint", nullable: false),
                    idpaciente = table.Column<int>(type: "int", nullable: false),
                    id_doctor = table.Column<int>(type: "int", nullable: false),
                    fechahora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_cita", x => x.idcita);
                    table.ForeignKey(
                        name: "FK_t_cita_t_doctor_id_doctor",
                        column: x => x.id_doctor,
                        principalTable: "t_doctor",
                        principalColumn: "id_doctor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_cita_t_paciente_idpaciente",
                        column: x => x.idpaciente,
                        principalTable: "t_paciente",
                        principalColumn: "idpaciente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_cita_t_usuario_idusuario",
                        column: x => x.idusuario,
                        principalTable: "t_usuario",
                        principalColumn: "idusuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_historial_medico",
                columns: table => new
                {
                    idhistorialmedico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idpaciente = table.Column<int>(type: "int", nullable: false),
                    notas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    diagnostico = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tratamientos = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cuadro_medico = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fechahora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_historial_medico", x => x.idhistorialmedico);
                    table.ForeignKey(
                        name: "FK_t_historial_medico_t_paciente_idpaciente",
                        column: x => x.idpaciente,
                        principalTable: "t_paciente",
                        principalColumn: "idpaciente",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_t_cita_id_doctor",
                table: "t_cita",
                column: "id_doctor");

            migrationBuilder.CreateIndex(
                name: "IX_t_cita_idpaciente",
                table: "t_cita",
                column: "idpaciente");

            migrationBuilder.CreateIndex(
                name: "IX_t_cita_idusuario",
                table: "t_cita",
                column: "idusuario");

            migrationBuilder.CreateIndex(
                name: "IX_t_doctor_idusuario",
                table: "t_doctor",
                column: "idusuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_doctor_especializacion_id_doctor",
                table: "t_doctor_especializacion",
                column: "id_doctor");

            migrationBuilder.CreateIndex(
                name: "IX_t_doctor_especializacion_idespecializacion",
                table: "t_doctor_especializacion",
                column: "idespecializacion");

            migrationBuilder.CreateIndex(
                name: "IX_t_historial_medico_idpaciente",
                table: "t_historial_medico",
                column: "idpaciente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_paciente_idusuario",
                table: "t_paciente",
                column: "idusuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_usuario_idrol",
                table: "t_usuario",
                column: "idrol");
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
