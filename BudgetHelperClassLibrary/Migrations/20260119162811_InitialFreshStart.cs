using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetHelperClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialFreshStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "BudgetForPurchases",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        CategoryId = table.Column<int>(type: "int", nullable: true),
            //        Period = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BudgetForPurchases", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Expenses",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CategoryId = table.Column<int>(type: "int", nullable: false),
            //        IsRecurring = table.Column<bool>(type: "bit", nullable: false),
            //        FrequencyInMonths = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Expenses", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "IncomeSources",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SourceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_IncomeSources", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CategoryExpense",
            //    columns: table => new
            //    {
            //        CategoryListId = table.Column<int>(type: "int", nullable: false),
            //        ExpensesListId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CategoryExpense", x => new { x.CategoryListId, x.ExpensesListId });
            //        table.ForeignKey(
            //            name: "FK_CategoryExpense_Categories_CategoryListId",
            //            column: x => x.CategoryListId,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_CategoryExpense_Expenses_ExpensesListId",
            //            column: x => x.ExpensesListId,
            //            principalTable: "Expenses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Incomes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IncomeSourceId = table.Column<int>(type: "int", nullable: false),
            //        ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CategoryId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Incomes", x => x.Id);
                    
            //        table.ForeignKey(
            //            name: "FK_Incomes_IncomeSources_IncomeSourceId",
            //            column: x => x.IncomeSourceId,
            //            principalTable: "IncomeSources",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CategoryExpense_ExpensesListId",
            //    table: "CategoryExpense",
            //    column: "ExpensesListId");

           

            //migrationBuilder.CreateIndex(
            //    name: "IX_Incomes_IncomeSourceId",
            //    table: "Incomes",
            //    column: "IncomeSourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BudgetForPurchases");

            //migrationBuilder.DropTable(
            //    name: "CategoryExpense");

            //migrationBuilder.DropTable(
            //    name: "Incomes");

            //migrationBuilder.DropTable(
            //    name: "Expenses");

            //migrationBuilder.DropTable(
            //    name: "Categories");

            //migrationBuilder.DropTable(
            //    name: "IncomeSources");
        }
    }
}
