using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    latitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    longitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    createdon = table.Column<DateTime>(name: "created_on", type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(name: "created_by", type: "character varying(200)", maxLength: 200, nullable: false),
                    lastmodifiedon = table.Column<DateTime>(name: "last_modified_on", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(name: "last_modified_by", type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transportation_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    createdon = table.Column<DateTime>(name: "created_on", type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(name: "created_by", type: "character varying(200)", maxLength: 200, nullable: false),
                    lastmodifiedon = table.Column<DateTime>(name: "last_modified_on", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(name: "last_modified_by", type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transportation_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "journeys",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "uuid", nullable: false),
                    startinglocationid = table.Column<Guid>(name: "starting_location_id", type: "uuid", nullable: false),
                    arrivallocationid = table.Column<Guid>(name: "arrival_location_id", type: "uuid", nullable: false),
                    startingtime = table.Column<DateTime>(name: "starting_time", type: "timestamp with time zone", nullable: false),
                    arrivaltime = table.Column<DateTime>(name: "arrival_time", type: "timestamp with time zone", nullable: false),
                    transportationtypeid = table.Column<Guid>(name: "transportation_type_id", type: "uuid", nullable: false),
                    routedistance = table.Column<double>(name: "route_distance", type: "double precision", nullable: false),
                    isgoalachieved = table.Column<bool>(name: "is_goal_achieved", type: "boolean", nullable: false, defaultValue: false),
                    locationid = table.Column<Guid>(name: "location_id", type: "uuid", nullable: true),
                    createdon = table.Column<DateTime>(name: "created_on", type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<string>(name: "created_by", type: "character varying(200)", maxLength: 200, nullable: false),
                    lastmodifiedon = table.Column<DateTime>(name: "last_modified_on", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<string>(name: "last_modified_by", type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_journeys", x => x.id);
                    table.ForeignKey(
                        name: "fk_journeys_locations_arrival_location_id",
                        column: x => x.arrivallocationid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_journeys_locations_location_id",
                        column: x => x.locationid,
                        principalTable: "locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_journeys_locations_starting_location_id",
                        column: x => x.startinglocationid,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_journeys_arrival_location_id",
                table: "journeys",
                column: "arrival_location_id");

            migrationBuilder.CreateIndex(
                name: "ix_journeys_location_id",
                table: "journeys",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "ix_journeys_starting_location_id",
                table: "journeys",
                column: "starting_location_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "journeys");

            migrationBuilder.DropTable(
                name: "transportation_types");

            migrationBuilder.DropTable(
                name: "locations");
        }
    }
}
