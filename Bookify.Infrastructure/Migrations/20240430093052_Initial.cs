using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "apartments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    address_Country = table.Column<string>(type: "text", nullable: false),
                    address_State = table.Column<string>(type: "text", nullable: false),
                    address_ZipCode = table.Column<string>(type: "text", nullable: false),
                    address_City = table.Column<string>(type: "text", nullable: false),
                    address_Street = table.Column<string>(type: "text", nullable: false),
                    price_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    price_Currency = table.Column<string>(type: "text", nullable: false),
                    cleaningFee_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    cleaningFee_Currency = table.Column<string>(type: "text", nullable: false),
                    lastBookedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    amenities = table.Column<int[]>(type: "integer[]", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_apartments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    lastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    apartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    duration_Start = table.Column<DateOnly>(type: "date", nullable: false),
                    duration_End = table.Column<DateOnly>(type: "date", nullable: false),
                    priceForPeriod_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    priceForPeriod_Currency = table.Column<string>(type: "text", nullable: false),
                    cleaningFee_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    cleaningFee_Currency = table.Column<string>(type: "text", nullable: false),
                    amenitiesUpCharge_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    amenitiesUpCharge_Currency = table.Column<string>(type: "text", nullable: false),
                    totalPrice_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    totalPrice_Currency = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    createdOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    confirmedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rejectedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    canceledOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "fK_bookings_apartments_apartmentId",
                        column: x => x.apartmentId,
                        principalTable: "apartments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_bookings_user_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    apartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    bookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    createdOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fK_reviews_apartments_apartmentId",
                        column: x => x.apartmentId,
                        principalTable: "apartments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_reviews_bookings_bookingId",
                        column: x => x.bookingId,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_reviews_user_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_bookings_apartmentId",
                table: "bookings",
                column: "apartmentId");

            migrationBuilder.CreateIndex(
                name: "iX_bookings_userId",
                table: "bookings",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_reviews_apartmentId",
                table: "reviews",
                column: "apartmentId");

            migrationBuilder.CreateIndex(
                name: "iX_reviews_bookingId",
                table: "reviews",
                column: "bookingId");

            migrationBuilder.CreateIndex(
                name: "iX_reviews_userId",
                table: "reviews",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
