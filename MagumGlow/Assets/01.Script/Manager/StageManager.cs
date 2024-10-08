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
    public float enemiesPerBoss = 20f; // 확장성을 위해 적 처치 수 설정
    public Monster monster;

    public List<Stage> stages = new List<Stage>();
    public List<Toggle> bossStages = new List<Toggle>(); // 보스 토글 리스트

    private int currentStageIndex = 0;
    private float enemyKillCount = 0f;

    private float sliderIncrementPerKill;
    private Stage currentStage;
    private StageData StageData = new StageData();
    private int midBossIndex = 0; // 중간 보스 순서 관리 인덱스

    private static StageManager instance;

    private string saveFilePath;

    private void Awake()
    {
        // 다른 씬에서도 이 오브젝트가 유지되도록 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 이 오브젝트가 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 StageManager가 있을 경우 새로 만든 오브젝트를 파괴
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

    // 스테이지 설정
    private void SetupStage(Stage stage)
    {
        currentStage = stage;
        Debug.Log(currentStage.StageName);
        enemyKillCount = 0f;
        midBossIndex = 0; // 새로운 스테이지 시작 시 인덱스 초기화
        sliderIncrementPerKill = (10f / 6) / enemiesPerBoss;
        stageSlider.value = 0f;
        ResetBossToggles(); // 토글 리셋
    }

    // 적 처치 시 호출되는 함수
    public void OnEnemyKilled()
    {
        //팝 아니면 그냥 뒤로 땡겨? 풀 없어서 일단 뒤로 땡겼어

        enemyKillCount++;
        UpdateSlider();


        monster.SO.MonsterLV = (int)enemyKillCount;


        if (enemyKillCount >= enemiesPerBoss)
        {
            enemyKillCount = 0f;
        }
    }

    // 슬라이더 업데이트 및 보스 소환 체크
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

    // 중간 보스 소환
    private void SpawnMidBoss()
    {
        if (midBossIndex < bossStages.Count)
        {
            Debug.Log("Mid Boss Spawned");
            bossStages[midBossIndex].isOn = true; // 해당 인덱스의 토글을 활성화
            midBossIndex++; // 인덱스 증가
        }
    }

    // 최종 보스 소환
    private void SpawnFinalBoss()
    {
        Debug.Log("Final Boss Spawned");
        OnStageComplete();
    }

    // 스테이지 완료 시 호출
    public void OnStageComplete()
    {
        Debug.Log("Stage Complete");
        currentStageIndex++;

        //Monster SO Change
        //monster.SO = monsterData[currentStageIndex];

        if (currentStageIndex < stages.Count)
        {
            //SaveStageData(); // 진행 상태 저장
            SetupStage(stages[currentStageIndex]);
            SceneManager.LoadScene(stages[currentStageIndex].StageName);
        }
        else
        {
            Debug.Log("All stages completed!");
        }
    }

    // 스테이지 데이터 저장 (Json 사용)
    public void SaveStageData()
    {
        string json = JsonUtility.ToJson(StageData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Stage data saved.");
    }

    // 스테이지 데이터 불러오기
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

    // 모든 보스 토글을 리셋
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
    public int TotalSliderSteps = 10; // 슬라이더 단계 수 (보스 소환 구간)

    // 중간 보스 소환 구간인지 체크
    public bool IsMidBossStage(float sliderValue)
    {
        return sliderValue == 1.7f || sliderValue == 3.4f || sliderValue == 5f || sliderValue == 6.7f || sliderValue == 8.4f;
    }

    // 최종 보스 소환 구간인지 체크
    public bool IsFinalBossStage(float sliderValue)
    {
        return sliderValue == TotalSliderSteps;
    }
}

public class StageData
{

}