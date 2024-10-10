using System;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public MagicSwordStats playerData;
    public Stage curStageData; // Reference to the current stage data

    private string saveFilePath;
    private string stageSaveFilePath;

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

        // Android �������� ������ ���� ��� ����
        saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        stageSaveFilePath = Path.Combine(Application.persistentDataPath, "StageData.json");

        LoadPlayerData();
        LoadStageData(); // Load stage data when the game starts

        // ������ ���۵� �� ��� �ð��� ���� ��� �߰�
        CalculateGoldOverTime();
    }

    private void OnApplicationQuit()
    {
        // ���� ���� �� ���� �ð��� ���
        PlayerPrefs.SetString("LastQuitTime", DateTime.UtcNow.ToString());
    }

    private void CalculateGoldOverTime()
    {
        // ������ ���� �ð� ��������
        if (PlayerPrefs.HasKey("LastQuitTime"))
        {
            string lastQuitTimeString = PlayerPrefs.GetString("LastQuitTime");
            DateTime lastQuitTime = DateTime.Parse(lastQuitTimeString);

            // ���� �ð��� ������ ���� �ð� ������ ��� �ð� ���
            TimeSpan timeAway = DateTime.UtcNow - lastQuitTime;
            double secondsAway = timeAway.TotalSeconds;

            // 10�ʸ��� 10��徿 ����
            int goldEarned = (int)(secondsAway / 10) * 10;

            // ��� �߰�
            if (goldEarned > 0)
            {
                playerData.AddGold(goldEarned);
                Debug.Log($"������ ���� �ִ� ���� {secondsAway}�� ���. {goldEarned} ��带 ȹ���߽��ϴ�.");
            }
        }
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Player data saved to {saveFilePath}");
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

    public void SaveStageData()
    {
        string json = JsonUtility.ToJson(curStageData, true);
        File.WriteAllText(stageSaveFilePath, json);
        Debug.Log($"Stage data saved to {stageSaveFilePath}");
    }

    public void LoadStageData()
    {
        if (File.Exists(stageSaveFilePath))
        {
            string json = File.ReadAllText(stageSaveFilePath);
            JsonUtility.FromJsonOverwrite(json, curStageData);
            Debug.Log("Stage data loaded.");
        }
        else
        {
            Debug.LogWarning("No stage save file found. Creating new data.");
            SaveStageData();
        }
    }

    // �÷��̾� �̸� ���� �޼���
    public void SetPlayerName(string name)
    {
        playerData.playerName = name;
        SavePlayerData(); // �̸� ���� �� JSON ���Ϸ� ����
        Debug.Log($"Player name set to {name}");
    }
}
