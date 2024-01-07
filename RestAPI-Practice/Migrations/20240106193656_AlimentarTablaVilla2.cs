using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPIPractice.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 1, 6, 16, 36, 56, 488, DateTimeKind.Local).AddTicks(3665), new DateTime(2024, 1, 6, 16, 36, 56, 488, DateTimeKind.Local).AddTicks(3651) });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[] { 2, "", "Detalle de la Villa...", new DateTime(2024, 1, 6, 16, 36, 56, 488, DateTimeKind.Local).AddTicks(3668), new DateTime(2024, 1, 6, 16, 36, 56, 488, DateTimeKind.Local).AddTicks(3667), "", 40.0, "Premium Vista a la Piscina", 4, 150.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 1, 6, 16, 36, 35, 687, DateTimeKind.Local).AddTicks(1477), new DateTime(2024, 1, 6, 16, 36, 35, 687, DateTimeKind.Local).AddTicks(1464) });
        }
    }
}
