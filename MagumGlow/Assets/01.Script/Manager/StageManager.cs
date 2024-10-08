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
    public List<Toggle> bossStages = new List<Toggle>(); // 보스 토글 리스트

    private float sliderIncrementPerKill;
    public Stage curStageData;

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
        // LoadStageData(); // 이미 호출되므로 주석 처리
        Debug.Log(curStageData.currentStageIndex);
        if (curStageData.currentStageIndex >= stages.Count) // >=로 수정
        {
            curStageData.currentStageIndex = 0;
        }

        SetupStage(stages[curStageData.currentStageIndex]);

        // 슬라이더 값을 로드
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

    // 스테이지 설정
    private void SetupStage(Stage stage)
    {
        curStageData = stage;
        Debug.Log(curStageData.StageName);
        curStageData.enemyKillCount = 0f;
        curStageData.midBossIndex = 0; // 새로운 스테이지 시작 시 인덱스 초기화
        sliderIncrementPerKill = (10f / 6) / curStageData.enemiesPerBoss;
        stageSlider.value = 0f;
        ResetBossToggles(); // 토글 리셋
    }

    // 적 처치 시 호출되는 함수
    public void OnEnemyKilled()
    {
        curStageData.enemyKillCount++;
        curStageData.sliderEnemyCount++;
        UpdateSlider();

        monster.SO.MonsterLV = (int)curStageData.enemyKillCount;

        // 적 처치 수가 보스 수와 같아지면 카운트를 리셋
        if (curStageData.enemyKillCount >= curStageData.enemiesPerBoss)
        {
            curStageData.enemyKillCount = 0f;
        }

        // Json 저장
        SaveStageData();
    }

    // 슬라이더 업데이트 및 보스 소환 체크
    private void UpdateSlider()
    {
        stageSlider.value = Mathf.Round((stageSlider.value + sliderIncrementPerKill) * 10f) / 10f;
        curStageData.stageSliderValue = stageSlider.value; // 슬라이더 값 저장

        if (curStageData.IsMidBossStage(stageSlider.value))
        {
            SpawnMidBoss();
        }
        else if (curStageData.IsFinalBossStage(stageSlider.value))
        {
            SpawnFinalBoss();
        }
    }


    // 중간 보스 소환
    private void SpawnMidBoss()
    {
        if (curStageData.midBossIndex < bossStages.Count)
        {
            Debug.Log("Mid Boss Spawned");
            bossStages[curStageData.midBossIndex].isOn = true; // 해당 인덱스의 토글을 활성화
            curStageData.midBossIndex++; // 인덱스 증가
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
        curStageData.currentStageIndex++;

        //Monster SO Change
        //monster.SO = monsterData[currentStageIndex];

        if (curStageData.currentStageIndex < stages.Count)
        {
            //SaveStageData(); // 진행 상태 저장
            SetupStage(stages[curStageData.currentStageIndex]);
            SceneManager.LoadScene(stages[curStageData.currentStageIndex].StageName);
        }
        else
        {
            Debug.Log("All stages completed!");
        }
    }

    // 스테이지 데이터 저장 (Json 사용)
    public void SaveStageData()
    {
        string json = JsonUtility.ToJson(curStageData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Stage data saved.");
    }

    // 스테이지 데이터 불러오기
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

    // 모든 보스 토글을 리셋
    private void ResetBossToggles()
    {
        foreach (var toggle in bossStages)
        {
            toggle.isOn = false;
        }
    }
}
