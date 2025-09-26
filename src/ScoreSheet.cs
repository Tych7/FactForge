using System;
using System.Collections.Generic;

namespace DesktopApp;

public class ScoreSheet
{
    public string? PlayerName { get; set; }
    public int Score { get; set; }

    public void CreateScoreSheet()
    {
        // Use JsonHandler for a list of PlayerScore
        var handler = new JsonHandler<List<ScoreSheet>>();

        // Filename with datetime (e.g., ScoreSheet_20250926_1334.json)
        var fileName = $"ScoreSheet_{DateTime.Now:yyyyMMdd_HHmmss}.json";

        // Save the list to JSON
        handler.CreateJsonFile(fileName);
    }

}
