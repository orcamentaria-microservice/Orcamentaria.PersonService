using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Orcamentaria.PersonService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _202611041200_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_PERSON",
                columns: table => new
                {
                    ID = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(type: "VARCHAR(70)", nullable: false),
                    RG = table.Column<string>(type: "VARCHAR(9)", nullable: true),
                    CPF = table.Column<string>(type: "VARCHAR(11)", nullable: true),
                    CNPJ = table.Column<string>(type: "VARCHAR(14)", nullable: true),
                    TYPE = table.Column<int>(type: "INT", nullable: false),
                    ACTIVE = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 1ul),
                    COMPANY_ID = table.Column<long>(type: "BIGINT", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CREATED_BY = table.Column<long>(type: "BIGINT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "BIGINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PERSON", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_ADDRESS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    STREET = table.Column<string>(type: "VARCHAR(70)", nullable: false),
                    ZIPCODE = table.Column<string>(type: "VARCHAR(8)", nullable: false),
                    NUMBER = table.Column<string>(type: "VARCHAR(6)", nullable: false),
                    COMPLEMENT = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    NEIGHBORHOOD = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CITY = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    STATE = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    UF = table.Column<string>(type: "VARCHAR(2)", nullable: false),
                    DEFAULT = table.Column<ulong>(type: "BIT", nullable: false),
                    PERSON_ID = table.Column<long>(type: "BIGINT", nullable: false),
                    COMPANY_ID = table.Column<long>(type: "BIGINT", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CREATED_BY = table.Column<long>(type: "BIGINT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "BIGINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ADDRESS", x => x.ID);
                    table.ForeignKey(
                        name: "fk_T_ADDRESS_T_PERSON",
                        column: x => x.PERSON_ID,
                        principalTable: "T_PERSON",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CONTACT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CONTACT = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    TYPE = table.Column<int>(type: "INT", nullable: false),
                    DEFAULT = table.Column<ulong>(type: "BIT", nullable: false),
                    PERSON_ID = table.Column<long>(type: "BIGINT", nullable: false),
                    COMPANY_ID = table.Column<long>(type: "BIGINT", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CREATED_BY = table.Column<long>(type: "BIGINT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "BIGINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CONTACT", x => x.ID);
                    table.ForeignKey(
                        name: "fk_T_CONTACT_T_PERSON",
                        column: x => x.PERSON_ID,
                        principalTable: "T_PERSON",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_EMPLOYEE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "BIGINT", nullable: false),
                    POST = table.Column<string>(type: "VARCHAR(60)", nullable: false),
                    ADMISSION_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    VALUE_PER_DAY = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_EMPLOYEE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_EMPLOYEE_T_PERSON_ID",
                        column: x => x.ID,
                        principalTable: "T_PERSON",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_T_ADDRESS_PERSON_ID",
                table: "T_ADDRESS",
                column: "PERSON_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CONTACT_PERSON_ID",
                table: "T_CONTACT",
                column: "PERSON_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_ADDRESS");

            migrationBuilder.DropTable(
                name: "T_CONTACT");

            migrationBuilder.DropTable(
                name: "T_EMPLOYEE");

            migrationBuilder.DropTable(
                name: "T_PERSON");
        }
    }
}
