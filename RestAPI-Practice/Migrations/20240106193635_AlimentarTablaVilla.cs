using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPIPractice.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[] { 1, "", "Detalle de la Villa...", new DateTime(2024, 1, 6, 16, 36, 35, 687, DateTimeKind.Local).AddTicks(1477), new DateTime(2024, 1, 6, 16, 36, 35, 687, DateTimeKind.Local).AddTicks(1464), "", 50.0, "Villa Real", 5, 200.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
