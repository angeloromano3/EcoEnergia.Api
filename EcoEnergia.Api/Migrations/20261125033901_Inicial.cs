using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoEnergia.Api.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_ECO_UNIDADE",
                columns: table => new
                {
                    CD_UNIDADE = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_UNIDADE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SG_ESTADO = table.Column<string>(type: "CHAR(2)", nullable: false),
                    NM_CIDADE = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    DS_SEGMENTO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ST_UNIDADE = table.Column<string>(type: "CHAR(1)", nullable: false),
                    DT_CADASTRO = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ECO_UNIDADE", x => x.CD_UNIDADE);
                });

            migrationBuilder.CreateTable(
                name: "TB_ECO_USUARIO",
                columns: table => new
                {
                    NM_USUARIO = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    DS_SENHA_HASH = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    ST_ATIVO = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ECO_USUARIO", x => x.NM_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_ECO_DISPOSITIVO",
                columns: table => new
                {
                    CD_DISPOSITIVO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CD_UNIDADE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NM_DISPOSITIVO = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    DS_TIPO_DISPOSITIVO = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: false),
                    VL_LIMITE_KWH_HORA = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    ST_DISPOSITIVO = table.Column<string>(type: "CHAR(1)", nullable: false),
                    DT_INSTALACAO = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ECO_DISPOSITIVO", x => x.CD_DISPOSITIVO);
                    table.ForeignKey(
                        name: "FK_TB_ECO_DISPOSITIVO_TB_ECO_UNIDADE_CD_UNIDADE",
                        column: x => x.CD_UNIDADE,
                        principalTable: "TB_ECO_UNIDADE",
                        principalColumn: "CD_UNIDADE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ECO_META_ENERGIA",
                columns: table => new
                {
                    CD_META = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CD_UNIDADE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NR_MES = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NR_ANO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VL_META_MAX_KWH = table.Column<decimal>(type: "NUMBER(12,2)", nullable: false),
                    VL_KWH_ACUMULADO = table.Column<decimal>(type: "NUMBER(12,2)", nullable: false),
                    ST_META = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ECO_META_ENERGIA", x => x.CD_META);
                    table.ForeignKey(
                        name: "FK_TB_ECO_META_ENERGIA_TB_ECO_UNIDADE_CD_UNIDADE",
                        column: x => x.CD_UNIDADE,
                        principalTable: "TB_ECO_UNIDADE",
                        principalColumn: "CD_UNIDADE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ECO_ALERTA_INCIDENTE",
                columns: table => new
                {
                    CD_ALERTA = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CD_UNIDADE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CD_DISPOSITIVO = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DT_ALERTA = table.Column<DateTime>(type: "DATE", nullable: false),
                    TP_SEVERIDADE = table.Column<string>(type: "NVARCHAR2(15)", maxLength: 15, nullable: false),
                    DS_MOTIVO = table.Column<string>(type: "NVARCHAR2(400)", maxLength: 400, nullable: false),
                    ST_RESOLUCAO = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ECO_ALERTA_INCIDENTE", x => x.CD_ALERTA);
                    table.ForeignKey(
                        name: "FK_TB_ECO_ALERTA_INCIDENTE_TB_ECO_DISPOSITIVO_CD_DISPOSITIVO",
                        column: x => x.CD_DISPOSITIVO,
                        principalTable: "TB_ECO_DISPOSITIVO",
                        principalColumn: "CD_DISPOSITIVO");
                    table.ForeignKey(
                        name: "FK_TB_ECO_ALERTA_INCIDENTE_TB_ECO_UNIDADE_CD_UNIDADE",
                        column: x => x.CD_UNIDADE,
                        principalTable: "TB_ECO_UNIDADE",
                        principalColumn: "CD_UNIDADE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ECO_CONSUMO_DIARIO",
                columns: table => new
                {
                    CD_CONSUMO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CD_DISPOSITIVO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DT_MEDICAO = table.Column<DateTime>(type: "DATE", nullable: false),
                    VL_KWH_CONSUMIDO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    VL_CUSTO_ESTIMADO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    ST_PROCESSADO = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ECO_CONSUMO_DIARIO", x => x.CD_CONSUMO);
                    table.ForeignKey(
                        name: "FK_TB_ECO_CONSUMO_DIARIO_TB_ECO_DISPOSITIVO_CD_DISPOSITIVO",
                        column: x => x.CD_DISPOSITIVO,
                        principalTable: "TB_ECO_DISPOSITIVO",
                        principalColumn: "CD_DISPOSITIVO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ECO_ALERTA_INCIDENTE_CD_DISPOSITIVO",
                table: "TB_ECO_ALERTA_INCIDENTE",
                column: "CD_DISPOSITIVO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ECO_ALERTA_INCIDENTE_CD_UNIDADE",
                table: "TB_ECO_ALERTA_INCIDENTE",
                column: "CD_UNIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ECO_CONSUMO_DIARIO_CD_DISPOSITIVO",
                table: "TB_ECO_CONSUMO_DIARIO",
                column: "CD_DISPOSITIVO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ECO_DISPOSITIVO_CD_UNIDADE",
                table: "TB_ECO_DISPOSITIVO",
                column: "CD_UNIDADE");

            migrationBuilder.CreateIndex(
                name: "UN_ECO_META_UNIDADE_MES_ANO",
                table: "TB_ECO_META_ENERGIA",
                columns: new[] { "CD_UNIDADE", "NR_MES", "NR_ANO" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ECO_ALERTA_INCIDENTE");

            migrationBuilder.DropTable(
                name: "TB_ECO_CONSUMO_DIARIO");

            migrationBuilder.DropTable(
                name: "TB_ECO_META_ENERGIA");

            migrationBuilder.DropTable(
                name: "TB_ECO_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_ECO_DISPOSITIVO");

            migrationBuilder.DropTable(
                name: "TB_ECO_UNIDADE");
        }
    }
}
