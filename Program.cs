// Personal Finance Tracker
// A simple console app to track income and expenses.
// Data is saved to a JSON file so it persists between sessions.

using PersonalFinanceTracker;

// Load any existing transactions from the file when the app starts
List<Transaction> transactions = FileHelper.Load();

// Keep showing the menu until the user chooses to exit
while (true)
{
    Console.Clear();
    ShowMenu();

    string choice = Console.ReadLine() ?? "";

    if (choice == "1") AddTransaction();
    else if (choice == "2") ListTransactions();
    else if (choice == "3") EditTransaction();
    else if (choice == "4") DeleteTransaction();
    else if (choice == "5") ShowSummary();
    else if (choice == "0") break;
    else
    {
        Console.WriteLine("Invalid option. Press any key to try again.");
        Console.ReadKey();
    }
}

Console.WriteLine("Goodbye! Your data has been saved.");


// ─────────────────────────────────────────────
// MENU
// ─────────────────────────────────────────────

void ShowMenu()
{
    Console.WriteLine("============================");
    Console.WriteLine("   PERSONAL FINANCE TRACKER");
    Console.WriteLine("============================");
    Console.WriteLine("[1] Add Transaction");
    Console.WriteLine("[2] View All Transactions");
    Console.WriteLine("[3] Edit Transaction");
    Console.WriteLine("[4] Delete Transaction");
    Console.WriteLine("[5] Summary");
    Console.WriteLine("[0] Exit");
    Console.WriteLine("============================");
    Console.Write("Choose an option: ");
}


// ─────────────────────────────────────────────
// CREATE — Add a new transaction
// ─────────────────────────────────────────────

void AddTransaction()
{
    Console.Clear();
    Console.WriteLine("--- Add Transaction ---\n");

    // Create a new transaction and fill in each field
    Transaction t = new Transaction();

    Console.Write("Description: ");
    t.Description = Console.ReadLine() ?? "";

    Console.Write("Amount: ");
    // Keep asking until the user enters a valid number
    while (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
    {
        Console.Write("Please enter a valid amount: ");
    }
    t.Amount = decimal.Parse(Console.ReadLine() ?? "0");

    // Let the user pick Income or Expense
    Console.WriteLine("Type: [1] Income  [2] Expense");
    Console.Write("Choice: ");
    t.Type = Console.ReadLine() == "1" ? "Income" : "Expense";

    Console.Write("Category (e.g. Food, Salary, Transport): ");
    t.Category = Console.ReadLine() ?? "General";

    // Add the new transaction to our list
    transactions.Add(t);

    // Save the updated list to the file immediately
    FileHelper.Save(transactions);

    Console.WriteLine("\nTransaction added successfully!");
    Console.WriteLine("Press any key to continue.");
    Console.ReadKey();
}


// ─────────────────────────────────────────────
// READ — Show all transactions
// ─────────────────────────────────────────────

void ListTransactions()
{
    Console.Clear();
    Console.WriteLine("--- All Transactions ---\n");

    if (transactions.Count == 0)
    {
        Console.WriteLine("No transactions found.");
    }
    else
    {
        // Print a simple header row
        Console.WriteLine($"{"#",-4} {"ID",-10} {"Date",-12} {"Type",-9} {"Category",-15} {"Description",-20} {"Amount",10}");
        Console.WriteLine(new string('-', 84));

        // Loop through every transaction and print one line each
        for (int i = 0; i < transactions.Count; i++)
        {
            Transaction t = transactions[i];

            // Show + for income and - for expense to make it clear at a glance
            string sign = t.Type == "Income" ? "+" : "-";

            Console.WriteLine($"{i + 1,-4} {t.Id[..8],-10} {t.Date:yyyy-MM-dd}  {t.Type,-9} {t.Category,-15} {t.Description,-20} {sign}{t.Amount,9:F2}");
        }
    }

    Console.WriteLine("\nPress any key to continue.");
    Console.ReadKey();
}


// ─────────────────────────────────────────────
// UPDATE — Edit an existing transaction
// ─────────────────────────────────────────────

void EditTransaction()
{
    Console.Clear();
    Console.WriteLine("--- Edit Transaction ---\n");

    // First show the list so the user knows which number to pick
    ListTransactionsInline();

    Console.Write("\nEnter the number of the transaction to edit (or 0 to cancel): ");
    if (!int.TryParse(Console.ReadLine(), out int index) || index == 0)
        return;

    // Convert from 1-based display number to 0-based list index
    index = index - 1;

    if (index < 0 || index >= transactions.Count)
    {
        Console.WriteLine("Invalid number.");
        Console.ReadKey();
        return;
    }

    Transaction t = transactions[index];

    Console.WriteLine($"\nEditing: {t.Description} ({t.Type}, {t.Amount:C})");
    Console.WriteLine("Press ENTER to keep the current value.\n");

    // For each field, show the current value and let the user change it
    Console.Write($"Description [{t.Description}]: ");
    string desc = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(desc)) t.Description = desc;

    Console.Write($"Amount [{t.Amount}]: ");
    string amtInput = Console.ReadLine() ?? "";
    if (decimal.TryParse(amtInput, out decimal newAmt) && newAmt > 0)
        t.Amount = newAmt;

    Console.Write($"Type (Income/Expense) [{t.Type}]: ");
    string typeInput = Console.ReadLine() ?? "";
    if (typeInput == "Income" || typeInput == "Expense")
        t.Type = typeInput;

    Console.Write($"Category [{t.Category}]: ");
    string cat = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(cat)) t.Category = cat;

    // Save the changes to the file
    FileHelper.Save(transactions);

    Console.WriteLine("\nTransaction updated! Press any key to continue.");
    Console.ReadKey();
}


// ─────────────────────────────────────────────
// DELETE — Remove a transaction
// ─────────────────────────────────────────────

void DeleteTransaction()
{
    Console.Clear();
    Console.WriteLine("--- Delete Transaction ---\n");

    ListTransactionsInline();

    Console.Write("\nEnter the number to delete (or 0 to cancel): ");
    if (!int.TryParse(Console.ReadLine(), out int index) || index == 0)
        return;

    index = index - 1;

    if (index < 0 || index >= transactions.Count)
    {
        Console.WriteLine("Invalid number.");
        Console.ReadKey();
        return;
    }

    Transaction t = transactions[index];

    // Ask the user to confirm before deleting
    Console.WriteLine($"\nAre you sure you want to delete \"{t.Description}\" ({t.Amount:C})? (yes/no)");
    string confirm = Console.ReadLine() ?? "";

    if (confirm.ToLower() == "yes" || confirm.ToLower() == "y")
    {
        transactions.RemoveAt(index);
        FileHelper.Save(transactions);
        Console.WriteLine("Deleted successfully.");
    }
    else
    {
        Console.WriteLine("Cancelled.");
    }

    Console.WriteLine("Press any key to continue.");
    Console.ReadKey();
}


// ─────────────────────────────────────────────
// SUMMARY — Show totals
// ─────────────────────────────────────────────

void ShowSummary()
{
    Console.Clear();
    Console.WriteLine("--- Summary ---\n");

    decimal totalIncome   = 0;
    decimal totalExpenses = 0;

    // Loop through all transactions and add up income vs expenses
    foreach (Transaction t in transactions)
    {
        if (t.Type == "Income")
            totalIncome += t.Amount;
        else
            totalExpenses += t.Amount;
    }

    decimal balance = totalIncome - totalExpenses;

    Console.WriteLine($"Total Income  : +{totalIncome:F2}");
    Console.WriteLine($"Total Expenses:  -{totalExpenses:F2}");
    Console.WriteLine($"Balance       :  {balance:F2}");

    if (balance >= 0)
        Console.WriteLine("\nYou are in the green!");
    else
        Console.WriteLine("\nYou are spending more than you earn.");

    Console.WriteLine("\nPress any key to continue.");
    Console.ReadKey();
}


// ─────────────────────────────────────────────
// HELPER — Print list without pausing
// Used inside Edit and Delete so we don't double-pause
// ─────────────────────────────────────────────

void ListTransactionsInline()
{
    if (transactions.Count == 0)
    {
        Console.WriteLine("No transactions found.");
        return;
    }

    Console.WriteLine($"{"#",-4} {"Description",-20} {"Type",-9} {"Amount",10}");
    Console.WriteLine(new string('-', 48));

    for (int i = 0; i < transactions.Count; i++)
    {
        Transaction t = transactions[i];
        string sign = t.Type == "Income" ? "+" : "-";
        Console.WriteLine($"{i + 1,-4} {t.Description,-20} {t.Type,-9} {sign}{t.Amount,9:F2}");
    }
}