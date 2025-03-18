using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerdata.json";

    public static void SavePlayer(Player player)
    {
        PlayerData data = new PlayerData(player);
        string json = JsonUtility.ToJson(data, true); // Convert to JSON format
        File.WriteAllText(savePath, json); // Save to file
        Debug.Log("Game Saved: " + savePath);
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Game Loaded! " + savePath);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}
