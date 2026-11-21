using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Seats = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Guests = table.Column<int>(type: "integer", nullable: false),
                    TableId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Round bread baked with a thick layer of melted Imeretian cheese inside, served hot from the oven.", "Khachapuri Imeruli", 18m },
                    { 2, "Boat-shaped bread filled with sulguni cheese, topped with a raw egg yolk and a piece of butter — stir it all together and tear off the crust.", "Adjarian Khachapuri", 22m },
                    { 3, "Hand-pleated dumplings filled with minced beef and pork, onion, coriander and hot broth, sprinkled with black pepper.", "Khinkali (5 pcs)", 12m },
                    { 4, "Dumplings filled with chopped mushrooms, onion and herbs, cooked in their own juices.", "Mushroom Khinkali (5 pcs)", 11m },
                    { 5, "Red beans stewed with onion, coriander, garlic and blue fenugreek, served bubbling in a clay pot with hot mchadi.", "Lobio in a clay pot", 14m },
                    { 6, "Crispy fried cornbread from western Georgia, made from maize flour, water and salt.", "Mchadi", 4m },
                    { 7, "Three balls of finely chopped spinach, beetroot and leek mixed with walnut paste, garlic and herbs, decorated with pomegranate seeds.", "Pkhali", 13m },
                    { 8, "Fried aubergine slices rolled around a garlicky walnut paste, topped with pomegranate seeds.", "Aubergine with walnuts", 15m },
                    { 9, "Ripe tomatoes and cucumbers with red onion, coriander, basil and a dressing of ground walnuts and Kakhetian sunflower oil.", "Tomato salad", 12m },
                    { 10, "Summer stew of aubergine, peppers, tomatoes, potatoes and onion with garlic and plenty of fresh herbs.", "Ajapsandali", 14m },
                    { 11, "Thick cornmeal porridge stretched with lots of melted sulguni cheese, a Megrelian classic.", "Elarji", 10m },
                    { 12, "Pork on a skewer grilled over vine branches, served with raw onion rings and tkemali plum sauce.", "Mtsvadi", 25m },
                    { 13, "Roasted young chicken baked in a clay pan in a creamy garlic and milk sauce.", "Chicken Shkmeruli", 28m },
                    { 14, "Lamb stewed with tarragon, green plums, white wine and spring onion.", "Chakapuli", 30m },
                    { 15, "Fried pork with potatoes and onion, served sizzling in a ketsi clay dish.", "Ojakhuri", 24m },
                    { 16, "Thick beef soup with rice, ground walnuts, tkemali plum sauce and khmeli-suneli spices.", "Kharcho", 16m },
                    { 17, "Walnuts threaded on a string and dipped in thickened grape juice, then dried — the Georgian sweet.", "Churchkhela", 6m },
                    { 18, "Grape pudding made from grape juice and corn flour, served chilled with crushed walnuts.", "Pelamushi", 7m }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "Number", "Seats" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 2, 2 },
                    { 3, 3, 2 },
                    { 4, 4, 4 },
                    { 5, 5, 4 },
                    { 6, 6, 4 },
                    { 7, 7, 6 },
                    { 8, 8, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TableId",
                table: "Bookings",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_Number",
                table: "Tables",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
