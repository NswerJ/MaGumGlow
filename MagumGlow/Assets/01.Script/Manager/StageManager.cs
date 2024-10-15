using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StageManager : MonoBehaviour
{
    public Slider stageSlider;
    public Monster monster;
    public Monster midBoss;
    public bool isBoss;

    public List<Stage> stages = new List<Stage>();
    public List<Toggle> bossStages = new List<Toggle>();
    public TextMeshProUGUI collectionGoldTxt;

    private float sliderIncrementPerKill;


    private void Start()
    {
        InitializeStage();
        InitializeSlider();
        
        //³ªÁß¿¡ ¼öÁ¤ÇÒ°Å
        monster.GetCompo<MonsterHP>().Dead += OnEnemyKilled; 
        midBoss.GetCompo<MonsterHP>().Dead += OnEnemyKilled;
        MonsterActive();
    }

    private void MonsterActive()
    {
        monster.gameObject.SetActive(!isBoss);
        midBoss.gameObject.SetActive(isBoss);
    }

    private void InitializeStage()
    {
        var curStageData = GameManager.Instance.curStageData;

        collectionGoldTxt.text = $"{GameManager.Instance.collectionGold} °ñµå¸¦\nÈ¹µæ ÇÏ¼Ì½À´Ï´Ù.";

        if (curStageData.currentStageIndex >= stages.Count)
        {
            curStageData.currentStageIndex = 0;
        }

        SetupStage(stages[curStageData.currentStageIndex]);
    }

    private void InitializeSlider()
    {
        var curStageData = GameManager.Instance.curStageData;
        stageSlider.value = curStageData.stageSliderValue;
    }

    private void SetupStage(Stage stage)
    {
        var curStageData = GameManager.Instance.curStageData;
        curStageData = stage;

        Debug.Log($"Setting up stage: {curStageData.StageName}");

        curStageData.enemyKillCount = 0f;
        curStageData.midBossIndex = 0;
        sliderIncrementPerKill = (10f / 6) / curStageData.enemiesPerBoss;
        stageSlider.value = 0f;

        ResetBossToggles();
    }

    public void OnEnemyKilled()
    {
        MonsterActive();

        var curStageData = GameManager.Instance.curStageData;

        curStageData.enemyKillCount++;
        curStageData.sliderEnemyCount++;
        UpdateSlider();

        monster.SO.MonsterLV++; 

        if (curStageData.enemyKillCount >= curStageData.enemiesPerBoss)
        {
            curStageData.enemyKillCount = 0f;
        }

        GameManager.Instance.SaveStageData(); 
    }

    private void UpdateSlider()
    {
        var curStageData = GameManager.Instance.curStageData;

        stageSlider.value = Mathf.Round((stageSlider.value + sliderIncrementPerKill) * 10f) / 10f;
        curStageData.stageSliderValue = stageSlider.value;

        if (curStageData.IsMidBossStage(stageSlider.value))
        {
            SpawnMidBoss();
        }
        else if (curStageData.IsFinalBossStage(stageSlider.value))
        {
            SpawnFinalBoss();
        }
        else isBoss = false;
    }

    private void SpawnMidBoss()
    {
        isBoss = true;
        MonsterActive();

        var curStageData = GameManager.Instance.curStageData;

        if (curStageData.midBossIndex < bossStages.Count)
        {
            Debug.Log("Mid Boss Spawned");
            bossStages[curStageData.midBossIndex].isOn = true;
            curStageData.midBossIndex++;
        }
        midBoss.SO.MonsterLV++;
    }

    private void SpawnFinalBoss()
    {
        Debug.Log("Final Boss Spawned");
        OnStageComplete(); 
    }

    public void CollectionGold()
    {
        GameManager.Instance.GameStartGold();
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

        GameManager.Instance.SaveStageData();
    }

    private void ResetBossToggles()
    {
        foreach (var toggle in bossStages)
        {
            toggle.isOn = false;
        }
    }
}
