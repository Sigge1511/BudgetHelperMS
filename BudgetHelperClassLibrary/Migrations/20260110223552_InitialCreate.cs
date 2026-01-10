using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetHelperClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetForPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetForPurchases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurringExpenseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomeSourceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurringExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrequencyInMonths = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringExpenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryExpense",
                columns: table => new
                {
                    CategoryListId = table.Column<int>(type: "int", nullable: false),
                    ExpensesListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryExpense", x => new { x.CategoryListId, x.ExpensesListId });
                    table.ForeignKey(
                        name: "FK_CategoryExpense_Categories_CategoryListId",
                        column: x => x.CategoryListId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryExpense_Expenses_ExpensesListId",
                        column: x => x.ExpensesListId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryIncome",
                columns: table => new
                {
                    CategoryListId = table.Column<int>(type: "int", nullable: false),
                    IncomesListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryIncome", x => new { x.CategoryListId, x.IncomesListId });
                    table.ForeignKey(
                        name: "FK_CategoryIncome_Categories_CategoryListId",
                        column: x => x.CategoryListId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryIncome_Incomes_IncomesListId",
                        column: x => x.IncomesListId,
                        principalTable: "Incomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomeIncomeSource",
                columns: table => new
                {
                    IncomeSourceListId = table.Column<int>(type: "int", nullable: false),
                    IncomesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeIncomeSource", x => new { x.IncomeSourceListId, x.IncomesId });
                    table.ForeignKey(
                        name: "FK_IncomeIncomeSource_IncomeSources_IncomeSourceListId",
                        column: x => x.IncomeSourceListId,
                        principalTable: "IncomeSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeIncomeSource_Incomes_IncomesId",
                        column: x => x.IncomesId,
                        principalTable: "Incomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseRecurringExpense",
                columns: table => new
                {
                    ExpenseListId = table.Column<int>(type: "int", nullable: false),
                    RecurringExpenseListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseRecurringExpense", x => new { x.ExpenseListId, x.RecurringExpenseListId });
                    table.ForeignKey(
                        name: "FK_ExpenseRecurringExpense_Expenses_ExpenseListId",
                        column: x => x.ExpenseListId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseRecurringExpense_RecurringExpenses_RecurringExpenseListId",
                        column: x => x.RecurringExpenseListId,
                        principalTable: "RecurringExpenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryExpense_ExpensesListId",
                table: "CategoryExpense",
                column: "ExpensesListId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryIncome_IncomesListId",
                table: "CategoryIncome",
                column: "IncomesListId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRecurringExpense_RecurringExpenseListId",
                table: "ExpenseRecurringExpense",
                column: "RecurringExpenseListId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeIncomeSource_IncomesId",
                table: "IncomeIncomeSource",
                column: "IncomesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetForPurchases");

            migrationBuilder.DropTable(
                name: "CategoryExpense");

            migrationBuilder.DropTable(
                name: "CategoryIncome");

            migrationBuilder.DropTable(
                name: "ExpenseRecurringExpense");

            migrationBuilder.DropTable(
                name: "IncomeIncomeSource");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "RecurringExpenses");

            migrationBuilder.DropTable(
                name: "IncomeSources");

            migrationBuilder.DropTable(
                name: "Incomes");
        }
    }
}
