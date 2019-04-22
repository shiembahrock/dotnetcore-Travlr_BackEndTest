using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    AmenitiesId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AmenitiesName = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.AmenitiesId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrencyCode = table.Column<string>(maxLength: 5, nullable: false),
                    CurrencyName = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Hotel",
                columns: table => new
                {
                    HotelId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HotelName = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 255, nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    CommissionRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotel", x => x.HotelId);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HotelId = table.Column<int>(nullable: false),
                    RoomName = table.Column<string>(maxLength: 50, nullable: false),
                    MaxOccupancy = table.Column<int>(nullable: false),
                    NetRate = table.Column<double>(nullable: false),
                    SellRate = table.Column<double>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomId);
                    table.ForeignKey(
                        name: "ForeignKey_Room_Currency",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_Room_Hotel",
                        column: x => x.HotelId,
                        principalTable: "Hotel",
                        principalColumn: "HotelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingRoom",
                columns: table => new
                {
                    BookingRoomId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookingCode = table.Column<string>(maxLength: 50, nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false),
                    CheckInDate = table.Column<DateTime>(nullable: false),
                    CheckOutDate = table.Column<DateTime>(nullable: false),
                    UniqueCode = table.Column<string>(maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRoom", x => x.BookingRoomId);
                    table.ForeignKey(
                        name: "ForeignKey_BookingRoom_Room",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomAmenities",
                columns: table => new
                {
                    RoomAmenitiesId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomId = table.Column<int>(nullable: false),
                    AmenitiesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAmenities", x => x.RoomAmenitiesId);
                    table.ForeignKey(
                        name: "ForeignKey_RoomAmenities_Amenities",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "AmenitiesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_RoomAmenities_Room",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_RoomId",
                table: "BookingRoom",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_CurrencyId",
                table: "Room",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_HotelId",
                table: "Room",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmenities_AmenitiesId",
                table: "RoomAmenities",
                column: "AmenitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmenities_RoomId",
                table: "RoomAmenities",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingRoom");

            migrationBuilder.DropTable(
                name: "RoomAmenities");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Hotel");
        }
    }
}
