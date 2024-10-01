using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public MagicSwordStats playerData;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.dataPath, "PlayerData.json");
        LoadPlayerData();
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Player data saved.");
    }

    public void LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            JsonUtility.FromJsonOverwrite(json, playerData);
            Debug.Log("Player data loaded.");
        }
        else
        {
            Debug.LogWarning("No save file found. Creating new data.");
            SavePlayerData();
        }
    }

    // 플레이어 이름 설정 메서드
    public void SetPlayerName(string name)
    {
        playerData.playerName = name;
        SavePlayerData(); // 이름 저장 후 JSON 파일로 저장
        Debug.Log($"Player name set to {name}");
    }
}

