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

        // Android 버전에서 데이터 저장 경로 설정
        saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        stageSaveFilePath = Path.Combine(Application.persistentDataPath, "StageData.json");

        LoadPlayerData();
        LoadStageData(); // Load stage data when the game starts

        // 게임이 시작될 때 경과 시간에 따라 골드 추가
        CalculateGoldOverTime();
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 시 현재 시간을 기록
        PlayerPrefs.SetString("LastQuitTime", DateTime.UtcNow.ToString());
    }

    private void CalculateGoldOverTime()
    {
        // 마지막 종료 시간 가져오기
        if (PlayerPrefs.HasKey("LastQuitTime"))
        {
            string lastQuitTimeString = PlayerPrefs.GetString("LastQuitTime");
            DateTime lastQuitTime = DateTime.Parse(lastQuitTimeString);

            // 현재 시간과 마지막 종료 시간 사이의 경과 시간 계산
            TimeSpan timeAway = DateTime.UtcNow - lastQuitTime;
            double secondsAway = timeAway.TotalSeconds;

            // 10초마다 10골드씩 지급
            int goldEarned = (int)(secondsAway / 10) * 10;

            // 골드 추가
            if (goldEarned > 0)
            {
                playerData.AddGold(goldEarned);
                Debug.Log($"게임이 꺼져 있는 동안 {secondsAway}초 경과. {goldEarned} 골드를 획득했습니다.");
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

    // 플레이어 이름 설정 메서드
    public void SetPlayerName(string name)
    {
        playerData.playerName = name;
        SavePlayerData(); // 이름 저장 후 JSON 파일로 저장
        Debug.Log($"Player name set to {name}");
    }
}
