using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string playerName;
    private string savePath;

    private void Awake()
    {
        // Ensure there's only one GameManager instance (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
            savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
            LoadPlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerData()
    {
        PlayerData data = new PlayerData
        {
            playerName = playerName
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            playerName = data.playerName;
        }
        else
        {
            playerName = null; // No saved name
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
}
