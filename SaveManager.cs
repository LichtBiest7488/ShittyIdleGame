using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;


public static class SaveManager
{
    //private static string saveFilePath = "C:\\Users\\Licht\\Desktop\\ShittyIdleGame\\IdleGameSave.json";
    private static string saveFilePath = ".\\IdleGameSave.json";

    public static void Save(SaveData saveData)
{
    try
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(saveData, options);
        File.WriteAllText(saveFilePath, json);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to save the game: {ex.Message}");
    }
}

public static SaveData Load()
{
    if (!File.Exists(saveFilePath)) return null;

    try
    {
        string json = File.ReadAllText(saveFilePath);
        return JsonSerializer.Deserialize<SaveData>(json);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to load the game: {ex.Message}");
        return null;
    }
}

}
