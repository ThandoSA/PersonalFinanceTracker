// This class handles saving and loading transactions from a JSON file.
// We use System.Text.Json which is built into .NET — no extra packages needed.

using System.Text.Json;

namespace PersonalFinanceTracker;

class FileHelper
{
    // The name of the file where we store our data
    private const string FileName = "transactions.json";

    // JSON options: WriteIndented makes the file human-readable
    private static JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    // SAVE: converts the list to JSON text and writes it to the file
    public static void Save(List<Transaction> transactions)
    {
        string json = JsonSerializer.Serialize(transactions, options);
        File.WriteAllText(FileName, json);
    }

    // LOAD: reads the JSON file and converts it back to a list
    // If the file doesn't exist yet, it just returns an empty list
    public static List<Transaction> Load()
    {
        if (!File.Exists(FileName))
            return new List<Transaction>();

        string json = File.ReadAllText(FileName);
        return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
    }
}