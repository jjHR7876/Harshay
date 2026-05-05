namespace ConsoleApp1;

public class LeaderboardEntry
{
    public string Name { get; set; }
    public int Score { get; set; }

    public LeaderboardEntry(string name, int score)
    {
        Name = name;
        Score = score;
    }
}

public class Leaderboard
{
    private List<LeaderboardEntry> entries;
    private string filePath;

    public Leaderboard(string filePath)
    {
        this.filePath = filePath;
        entries = new List<LeaderboardEntry>();
        Load();
    }

    // Load existing leaderboard from file
    // Format: Name,Score (one per line)
    private void Load()
    {
        if (File.Exists(filePath) == false)
        {
            return; // no file yet, start fresh
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (line.Trim() == "")
            {
                continue;
            }

            string[] parts = line.Split(',');

            if (parts.Length == 2)
            {
                string name = parts[0];
                int score = 0;
                int.TryParse(parts[1], out score);

                entries.Add(new LeaderboardEntry(name, score));
            }
        }
    }

    public void AddEntry(string name, int score)
    {
        entries.Add(new LeaderboardEntry(name, score));

        // Sort by score descending (highest first)
        // Simple bubble sort so it's easy to understand
        for (int i = 0; i < entries.Count - 1; i++)
        {
            for (int j = 0; j < entries.Count - i - 1; j++)
            {
                if (entries[j].Score < entries[j + 1].Score)
                {
                    LeaderboardEntry temp = entries[j];
                    entries[j] = entries[j + 1];
                    entries[j + 1] = temp;
                }
            }
        }

        // Keep only top 10
        while (entries.Count > 10)
        {
            entries.RemoveAt(entries.Count - 1);
        }
    }

    public void Save()
    {
        List<string> lines = new List<string>();

        for (int i = 0; i < entries.Count; i++)
        {
            lines.Add(entries[i].Name + "," + entries[i].Score);
        }

        File.WriteAllLines(filePath, lines);
    }

    public void Display()
    {
        Console.WriteLine("\n--- LEADERBOARD ---");

        if (entries.Count == 0)
        {
            Console.WriteLine("No entries yet.");
            return;
        }

        for (int i = 0; i < entries.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + entries[i].Name + " - $" + entries[i].Score);
        }
    }
}