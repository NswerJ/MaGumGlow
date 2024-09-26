using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<MonsterSO> enemies;
    [SerializeField] private List<GameObject> backgrounds;

    //UI 1-1 �̷���
    // ���� ����� �� �����̰� ���ְ���?
    public int currentStage = 1;
    private int stageIndex;

    public int currentLevel = 1;
    private int levelIndex;

    private void Awake()
    {
        LevelSetting();
    }

    public void LevelClear()
    {
        if (currentLevel >= 10)
        {
            currentStage++;
            currentLevel = 1;
        }
        else
            currentLevel++;

        LevelSetting();
    }

    public void LevelSetting()
    {

        enemies[currentStage].MonsterLV = currentLevel;

        if(currentLevel != levelIndex)
        {
            levelIndex = currentLevel;
        }

    }
}
