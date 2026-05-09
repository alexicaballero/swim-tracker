using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwimTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClubSwimmer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clubs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    acronym = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    country_code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    federation_member_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clubs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "swimmers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    club_id = table.Column<Guid>(type: "uuid", nullable: false),
                    keycloak_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nationality = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    license_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    license_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    license_expires_at = table.Column<DateOnly>(type: "date", nullable: true),
                    federation_athlete_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    guardian_consent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_swimmers", x => x.id);
                    table.ForeignKey(
                        name: "FK_swimmers_clubs_club_id",
                        column: x => x.club_id,
                        principalTable: "clubs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clubs_name",
                table: "clubs",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_swimmers_club_id",
                table: "swimmers",
                column: "club_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "swimmers");

            migrationBuilder.DropTable(
                name: "clubs");
        }
    }
}
