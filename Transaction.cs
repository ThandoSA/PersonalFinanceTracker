// This class represents a single financial transaction.
// It holds all the information about one income or expense entry.

namespace PersonalFinanceTracker;

class Transaction
{
    // Unique ID so we can find, edit, or delete a specific transaction
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // A short description, e.g. "Groceries" or "Monthly Salary"
    public string Description { get; set; } = "";

    // The amount of money (always positive)
    public decimal Amount { get; set; }

    // Either "Income" or "Expense"
    public string Type { get; set; } = "Expense";

    // A category to group transactions, e.g. "Food", "Transport"
    public string Category { get; set; } = "General";

    // When the transaction happened
    public DateTime Date { get; set; } = DateTime.Now;
}