using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEditor.U2D.Aseprite;

public class StageManager : MonoBehaviour
{
    public Slider stageSlider;
    public Monster monster;

    public List<Stage> stages = new List<Stage>();
    public List<Toggle> bossStages = new List<Toggle>(); // ���� ��� ����Ʈ

    private float sliderIncrementPerKill;
    public Stage curStageData;

    private static StageManager instance;

    private string saveFilePath;

    private void Awake()
    {
        // �ٸ� �������� �� ������Ʈ�� �����ǵ��� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϴ� StageManager�� ���� ��� ���� ���� ������Ʈ�� �ı�
        }

        saveFilePath = Path.Combine(Application.dataPath, "StageData.json");
        
        LoadStageData();
    }

    private void Start()
    {
        // LoadStageData(); // �̹� ȣ��ǹǷ� �ּ� ó��
        Debug.Log(curStageData.currentStageIndex);
        if (curStageData.currentStageIndex >= stages.Count) // >=�� ����
        {
            curStageData.currentStageIndex = 0;
        }

        SetupStage(stages[curStageData.currentStageIndex]);

        // �����̴� ���� �ε�
        stageSlider.value = curStageData.stageSliderValue;

        // Event Add
        monster.GetCompo<MonsterHP>().Dead += OnEnemyKilled;

        // Monster Setup
        // monster.SO = monsterData[currentStageIndex];
        // monster.SO.MonsterLV = (int)enemyKillCount + 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEnemyKilled();
            //OnStageComplete();
        }
    }

    // �������� ����
    private void SetupStage(Stage stage)
    {
        curStageData = stage;
        Debug.Log(curStageData.StageName);
        curStageData.enemyKillCount = 0f;
        curStageData.midBossIndex = 0; // ���ο� �������� ���� �� �ε��� �ʱ�ȭ
        sliderIncrementPerKill = (10f / 6) / curStageData.enemiesPerBoss;
        stageSlider.value = 0f;
        ResetBossToggles(); // ��� ����
    }

    // �� óġ �� ȣ��Ǵ� �Լ�
    public void OnEnemyKilled()
    {
        curStageData.enemyKillCount++;
        curStageData.sliderEnemyCount++;
        UpdateSlider();

        monster.SO.MonsterLV = (int)curStageData.enemyKillCount;

        // �� óġ ���� ���� ���� �������� ī��Ʈ�� ����
        if (curStageData.enemyKillCount >= curStageData.enemiesPerBoss)
        {
            curStageData.enemyKillCount = 0f;
        }

        // Json ����
        SaveStageData();
    }

    // �����̴� ������Ʈ �� ���� ��ȯ üũ
    private void UpdateSlider()
    {
        stageSlider.value = Mathf.Round((stageSlider.value + sliderIncrementPerKill) * 10f) / 10f;
        curStageData.stageSliderValue = stageSlider.value; // �����̴� �� ����

        if (curStageData.IsMidBossStage(stageSlider.value))
        {
            SpawnMidBoss();
        }
        else if (curStageData.IsFinalBossStage(stageSlider.value))
        {
            SpawnFinalBoss();
        }
    }


    // �߰� ���� ��ȯ
    private void SpawnMidBoss()
    {
        if (curStageData.midBossIndex < bossStages.Count)
        {
            Debug.Log("Mid Boss Spawned");
            bossStages[curStageData.midBossIndex].isOn = true; // �ش� �ε����� ����� Ȱ��ȭ
            curStageData.midBossIndex++; // �ε��� ����
        }
    }

    // ���� ���� ��ȯ
    private void SpawnFinalBoss()
    {
        Debug.Log("Final Boss Spawned");
        OnStageComplete();
    }

    // �������� �Ϸ� �� ȣ��
    public void OnStageComplete()
    {
        Debug.Log("Stage Complete");
        curStageData.currentStageIndex++;

        //Monster SO Change
        //monster.SO = monsterData[currentStageIndex];

        if (curStageData.currentStageIndex < stages.Count)
        {
            //SaveStageData(); // ���� ���� ����
            SetupStage(stages[curStageData.currentStageIndex]);
            SceneManager.LoadScene(stages[curStageData.currentStageIndex].StageName);
        }
        else
        {
            Debug.Log("All stages completed!");
        }
    }

    // �������� ������ ���� (Json ���)
    public void SaveStageData()
    {
        string json = JsonUtility.ToJson(curStageData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Stage data saved.");
    }

    // �������� ������ �ҷ�����
    public void LoadStageData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            JsonUtility.FromJsonOverwrite(json, curStageData);
            Debug.Log("Stage data loaded.");
        }
        else
        {
            Debug.LogWarning("No save file found. Creating new data.");
            SaveStageData();
        }
    }

    // ��� ���� ����� ����
    private void ResetBossToggles()
    {
        foreach (var toggle in bossStages)
        {
            toggle.isOn = false;
        }
    }
}
