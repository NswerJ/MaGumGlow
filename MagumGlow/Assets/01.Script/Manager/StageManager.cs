using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class StageManager : MonoBehaviour
{
    public Slider stageSlider;
    public float enemiesPerBoss = 20f; // Ȯ�强�� ���� �� óġ �� ����
    public Monster monster;

    public List<Stage> stages = new List<Stage>();
    public List<Toggle> bossStages = new List<Toggle>(); // ���� ��� ����Ʈ

    private int currentStageIndex = 0;
    private float enemyKillCount = 0f;

    private float sliderIncrementPerKill;
    private Stage currentStage;
    private StageData StageData = new StageData();
    private int midBossIndex = 0; // �߰� ���� ���� ���� �ε���

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
        //LoadStageData();
        Debug.Log(currentStageIndex);
        SetupStage(stages[currentStageIndex]);

        //Event Add
        monster.GetCompo<MonsterHP>().Dead += OnEnemyKilled;

        //Monster Setup
        //monster.SO = monsterData[currentStageIndex];
        monster.SO.MonsterLV = (int)enemyKillCount + 1;
    }

    private void OnDisable()
    {
        monster.GetCompo<MonsterHP>().Dead -= OnEnemyKilled;
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
        currentStage = stage;
        Debug.Log(currentStage.StageName);
        enemyKillCount = 0f;
        midBossIndex = 0; // ���ο� �������� ���� �� �ε��� �ʱ�ȭ
        sliderIncrementPerKill = (10f / 6) / enemiesPerBoss;
        stageSlider.value = 0f;
        ResetBossToggles(); // ��� ����
    }

    // �� óġ �� ȣ��Ǵ� �Լ�
    public void OnEnemyKilled()
    {
        //�� �ƴϸ� �׳� �ڷ� ����? Ǯ ��� �ϴ� �ڷ� �����

        enemyKillCount++;
        UpdateSlider();


        monster.SO.MonsterLV = (int)enemyKillCount;


        if (enemyKillCount >= enemiesPerBoss)
        {
            enemyKillCount = 0f;
        }
    }

    // �����̴� ������Ʈ �� ���� ��ȯ üũ
    private void UpdateSlider()
    {
        stageSlider.value = Mathf.Round((stageSlider.value + sliderIncrementPerKill) * 10f) / 10f;

        if (currentStage.IsMidBossStage(stageSlider.value))
        {
            SpawnMidBoss();
        }
        else if (currentStage.IsFinalBossStage(stageSlider.value))
        {
            SpawnFinalBoss();
        }
    }

    // �߰� ���� ��ȯ
    private void SpawnMidBoss()
    {
        if (midBossIndex < bossStages.Count)
        {
            Debug.Log("Mid Boss Spawned");
            bossStages[midBossIndex].isOn = true; // �ش� �ε����� ����� Ȱ��ȭ
            midBossIndex++; // �ε��� ����
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
        currentStageIndex++;

        //Monster SO Change
        //monster.SO = monsterData[currentStageIndex];

        if (currentStageIndex < stages.Count)
        {
            //SaveStageData(); // ���� ���� ����
            SetupStage(stages[currentStageIndex]);
            SceneManager.LoadScene(stages[currentStageIndex].StageName);
        }
        else
        {
            Debug.Log("All stages completed!");
        }
    }

    // �������� ������ ���� (Json ���)
    public void SaveStageData()
    {
        string json = JsonUtility.ToJson(StageData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Stage data saved.");
    }

    // �������� ������ �ҷ�����
    public void LoadStageData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            JsonUtility.FromJsonOverwrite(json, StageData);
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

[Serializable]
public class Stage
{
    public string StageName;
    public List<MonsterSO> monsters;
    public int TotalSliderSteps = 10; // �����̴� �ܰ� �� (���� ��ȯ ����)

    // �߰� ���� ��ȯ �������� üũ
    public bool IsMidBossStage(float sliderValue)
    {
        return sliderValue == 1.7f || sliderValue == 3.4f || sliderValue == 5f || sliderValue == 6.7f || sliderValue == 8.4f;
    }

    // ���� ���� ��ȯ �������� üũ
    public bool IsFinalBossStage(float sliderValue)
    {
        return sliderValue == TotalSliderSteps;
    }
}

public class StageData
{

}