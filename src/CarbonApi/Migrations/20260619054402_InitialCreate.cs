using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_EMPRESA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    CNPJ = table.Column<string>(type: "NVARCHAR2(18)", maxLength: 18, nullable: false),
                    SETOR = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PAIS = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ATIVA = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EMPRESA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_AGENDAMENTO_REDUCAO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPRESA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TITULO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    META_REDUCAO_TCO2E = table.Column<decimal>(type: "DECIMAL(15,4)", precision: 15, scale: 4, nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DATA_INICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_FIM_PREVISTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AGENDAMENTO_REDUCAO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_AGENDAMENTO_REDUCAO_TB_EMPRESA_EMPRESA_ID",
                        column: x => x.EMPRESA_ID,
                        principalTable: "TB_EMPRESA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_COMPENSACAO_CARBONO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPRESA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    QUANTIDADE_TCO2E = table.Column<decimal>(type: "DECIMAL(15,4)", precision: 15, scale: 4, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    CERTIFICADO_URL = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DATA_COMPENSACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_REGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COMPENSACAO_CARBONO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_COMPENSACAO_CARBONO_TB_EMPRESA_EMPRESA_ID",
                        column: x => x.EMPRESA_ID,
                        principalTable: "TB_EMPRESA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_EMISSAO_CARBONO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPRESA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    QUANTIDADE_TCO2E = table.Column<decimal>(type: "DECIMAL(15,4)", precision: 15, scale: 4, nullable: false),
                    ESCOPO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DATA_EMISSAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_REGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EMISSAO_CARBONO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_EMISSAO_CARBONO_TB_EMPRESA_EMPRESA_ID",
                        column: x => x.EMPRESA_ID,
                        principalTable: "TB_EMPRESA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    ROLE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    EMPRESA_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ATIVO = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_TB_EMPRESA_EMPRESA_ID",
                        column: x => x.EMPRESA_ID,
                        principalTable: "TB_EMPRESA",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "TB_EMPRESA",
                columns: new[] { "ID", "ATIVA", "CNPJ", "DATA_ATUALIZACAO", "DATA_CADASTRO", "NOME", "PAIS", "SETOR" },
                values: new object[] { 1L, true, "12.345.678/0001-99", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EcoTech Brasil", "Brasil", "Tecnologia" });

            migrationBuilder.InsertData(
                table: "TB_USUARIO",
                columns: new[] { "ID", "ATIVO", "DATA_CADASTRO", "EMAIL", "EMPRESA_ID", "NOME", "ROLE", "SENHA" },
                values: new object[] { 1L, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@esg.com", null, "Administrador", "ADMIN", "$2a$10$.KMru7cwP9MPAksH5rIg2eoEYTaOIZ3QFAekd5o6RZNZJGIPrmkkK" });

            migrationBuilder.CreateIndex(
                name: "IX_TB_AGENDAMENTO_REDUCAO_EMPRESA_ID",
                table: "TB_AGENDAMENTO_REDUCAO",
                column: "EMPRESA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMPENSACAO_CARBONO_EMPRESA_ID",
                table: "TB_COMPENSACAO_CARBONO",
                column: "EMPRESA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_EMISSAO_CARBONO_EMPRESA_ID",
                table: "TB_EMISSAO_CARBONO",
                column: "EMPRESA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_EMPRESA_CNPJ",
                table: "TB_EMPRESA",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_EMAIL",
                table: "TB_USUARIO",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_EMPRESA_ID",
                table: "TB_USUARIO",
                column: "EMPRESA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AGENDAMENTO_REDUCAO");

            migrationBuilder.DropTable(
                name: "TB_COMPENSACAO_CARBONO");

            migrationBuilder.DropTable(
                name: "TB_EMISSAO_CARBONO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_EMPRESA");
        }
    }
}
