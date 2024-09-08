using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mpss");

            migrationBuilder.CreateTable(
                name: "Counteragents",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Contact = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    INN = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    OGRN = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    AccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BIK = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counteragents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsDriver = table.Column<bool>(type: "boolean", nullable: false),
                    Passport = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    INN = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    AccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BIK = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cost = table.Column<double>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    DeadlineDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CounteragentId = table.Column<int>(type: "integer", nullable: true),
                    ResponsibleEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ManagerShare = table.Column<float>(type: "real", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ProjectStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Counteragents_CounteragentId",
                        column: x => x.CounteragentId,
                        principalSchema: "mpss",
                        principalTable: "Counteragents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_Employees_ResponsibleEmployeeId",
                        column: x => x.ResponsibleEmployeeId,
                        principalSchema: "mpss",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "mpss",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductComponents",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductComponents_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "mpss",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShifts",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Arrival = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Departure = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TravelTime = table.Column<float>(type: "float", nullable: true),
                    ConsiderTravel = table.Column<bool>(type: "boolean", nullable: false),
                    ISN = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "mpss",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "mpss",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<double>(type: "numeric(18,2)", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: true),
                    IsPaidByCompany = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "mpss",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "mpss",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFile",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    UploadDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFile_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "mpss",
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectProducts",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Markup = table.Column<double>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "mpss",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProducts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "mpss",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSuspensions",
                schema: "mpss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    DateSuspended = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSuspensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSuspensions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "mpss",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_EmployeeId",
                schema: "mpss",
                table: "EmployeeShifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ProjectId",
                schema: "mpss",
                table: "EmployeeShifts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_EmployeeId",
                schema: "mpss",
                table: "Expenses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ProjectId",
                schema: "mpss",
                table: "Expenses",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponents_ProductId",
                schema: "mpss",
                table: "ProductComponents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFile_ProjectId",
                schema: "mpss",
                table: "ProjectFile",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProducts_ProductId",
                schema: "mpss",
                table: "ProjectProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProducts_ProjectId",
                schema: "mpss",
                table: "ProjectProducts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CounteragentId",
                schema: "mpss",
                table: "Projects",
                column: "CounteragentId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ResponsibleEmployeeId",
                schema: "mpss",
                table: "Projects",
                column: "ResponsibleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSuspensions_ProjectId",
                schema: "mpss",
                table: "ProjectSuspensions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                schema: "mpss",
                table: "Users",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeShifts",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "ProductComponents",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "ProjectFile",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "ProjectProducts",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "ProjectSuspensions",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "Counteragents",
                schema: "mpss");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "mpss");
        }
    }
}
