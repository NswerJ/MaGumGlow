using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageManager : MonoBehaviour
{
    public Slider stageSlider;
    public Monster monster;
    public List<Stage> stages = new List<Stage>();
    public List<Toggle> bossStages = new List<Toggle>(); // º¸½º Åä±Û ¸®½ºÆ®
    public TextMeshProUGUI collectionGoldTxt;

    private float sliderIncrementPerKill;

    private void Awake()
    {

    }

    private void Start()
    {

        var curStageData = GameManager.Instance.curStageData;
        collectionGoldTxt.text = GameManager.Instance.collectionGold.ToString() + "°ñµå¸¦\nÈ¹µæ ÇÏ¼Ì½À´Ï´Ù.";
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

        if (curStageData.currentStageIndex >= stages.Count)
        {
            curStageData.currentStageIndex = 0;
        }

        SetupStage(stages[curStageData.currentStageIndex]);

        stageSlider.value = curStageData.stageSliderValue;
        // Event Add
        monster.GetCompo<MonsterHP>().Dead += OnEnemyKilled;
    }


    private void SetupStage(Stage stage)
    {
        var curStageData = GameManager.Instance.curStageData;
        curStageData = stage; // Setting the current stage data
        Debug.Log(curStageData.StageName);
        curStageData.enemyKillCount = 0f;
        curStageData.midBossIndex = 0;
        sliderIncrementPerKill = (10f / 6) / curStageData.enemiesPerBoss;
        stageSlider.value = 0f;
        ResetBossToggles();
    }

    public void OnEnemyKilled()
    {
        var curStageData = GameManager.Instance.curStageData;
        curStageData.enemyKillCount++;
        curStageData.sliderEnemyCount++;
        UpdateSlider();

        //LV UP
        monster.SO.MonsterLV++;

        if (curStageData.enemyKillCount >= curStageData.enemiesPerBoss)
        {
            curStageData.enemyKillCount = 0f;
        }

        // Save stage data after killing an enemy
        GameManager.Instance.SaveStageData();
    }

    private void UpdateSlider()
    {
        var curStageData = GameManager.Instance.curStageData;
        curStageData.stageSliderValue = stageSlider.value;
        stageSlider.value = Mathf.Round((stageSlider.value + sliderIncrementPerKill) * 10f) / 10f;

        if (curStageData.IsMidBossStage(stageSlider.value))
        {
            SpawnMidBoss();
        }
        else if (curStageData.IsFinalBossStage(stageSlider.value))
        {
            SpawnFinalBoss();
        }
    }

    private void SpawnMidBoss()
    {
        var curStageData = GameManager.Instance.curStageData;
        Debug.Log("Mid Boss Spawned");
        bossStages[curStageData.midBossIndex].isOn = true;
        curStageData.midBossIndex++;
    }
    public void CollectionGold()
    {
        GameManager.Instance.GameStartGold();
    }

    private void SpawnFinalBoss()
    {
        Debug.Log("Final Boss Spawned");
        OnStageComplete();
    }

    public void OnStageComplete()
    {
        var curStageData = GameManager.Instance.curStageData;
        Debug.Log("Stage Complete");
        curStageData.currentStageIndex++;
        curStageData.stageSliderValue = 0;
        if (curStageData.currentStageIndex < stages.Count)
        {
            SetupStage(stages[curStageData.currentStageIndex]);
            SceneManager.LoadScene(stages[curStageData.currentStageIndex].StageName);
        }
        else
        {
            Debug.Log("All stages completed!");
        }

        GameManager.Instance.SaveStageData(); // Save after completing a stage
    }

    private void ResetBossToggles()
    {
        foreach (var toggle in bossStages)
        {
            toggle.isOn = false;
        }
    }
}
