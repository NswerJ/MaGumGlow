using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageManager : MonoBehaviour
{
    public Slider stageSlider;
    public Monster monster;
    public Monster midBoss;
    public bool isBoss;

    public List<Stage> stages = new List<Stage>();
    public List<Toggle> bossStages = new List<Toggle>(); // ���� ��� ����Ʈ
    public TextMeshProUGUI collectionGoldTxt;

    private float sliderIncrementPerKill;

    private void Awake()
    {

    }

    private void Start()
    {

        var curStageData = GameManager.Instance.curStageData;
        collectionGoldTxt.text = GameManager.Instance.collectionGold.ToString() + "��带\nȹ�� �ϼ̽��ϴ�.";
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

        if (curStageData.currentStageIndex >= stages.Count)
        {
            curStageData.currentStageIndex = 0;
        }

        SetupStage(stages[curStageData.currentStageIndex]);

        stageSlider.value = curStageData.stageSliderValue;
        // Event Add,  Pool�� �ٲٱ�
        monster.GetCompo<MonsterHP>().Dead += OnEnemyKilled;
        midBoss.GetCompo<MonsterHP>().Dead += OnEnemyKilled;

        monster.gameObject.SetActive(!isBoss);
        midBoss.gameObject.SetActive(isBoss);
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
        //Pool�� �ٲٱ�
        monster.gameObject.SetActive(!isBoss);
        midBoss.gameObject.SetActive(isBoss);

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
        //Pool�� �ٲٱ�
        else
            isBoss = false;
    }

    private void SpawnMidBoss()
    {
        //Pool�� �ٲٱ�
        isBoss = true;

        monster.gameObject.SetActive(!isBoss);
        midBoss.gameObject.SetActive(isBoss);


        var curStageData = GameManager.Instance.curStageData;
        Debug.Log("Mid Boss Spawned");
        bossStages[curStageData.midBossIndex].isOn = true;
        curStageData.midBossIndex++;
        midBoss.SO.MonsterLV++;
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
