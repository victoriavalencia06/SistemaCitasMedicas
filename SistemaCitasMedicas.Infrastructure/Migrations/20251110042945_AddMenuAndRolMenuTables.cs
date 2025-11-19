using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaCitasMedicas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuAndRolMenuTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_menu",
                columns: table => new
                {
                    idmenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ruta = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    icono = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_menu", x => x.idmenu);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_rol_menu",
                columns: table => new
                {
                    idrolmenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idrol = table.Column<int>(type: "int", nullable: false),
                    idmenu = table.Column<int>(type: "int", nullable: false),
                    habilitado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_rol_menu", x => x.idrolmenu);
                    table.ForeignKey(
                        name: "FK_t_rol_menu_t_menu_idmenu",
                        column: x => x.idmenu,
                        principalTable: "t_menu",
                        principalColumn: "idmenu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_rol_menu_t_rol_idrol",
                        column: x => x.idrol,
                        principalTable: "t_rol",
                        principalColumn: "idrol",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_t_rol_menu_idmenu",
                table: "t_rol_menu",
                column: "idmenu");

            migrationBuilder.CreateIndex(
                name: "IX_t_rol_menu_idrol_idmenu",
                table: "t_rol_menu",
                columns: new[] { "idrol", "idmenu" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_rol_menu");

            migrationBuilder.DropTable(
                name: "t_menu");
        }
    }
}
