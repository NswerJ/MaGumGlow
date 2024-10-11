using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Stage")]
public class Stage : ScriptableObject
{
    public string StageName;
    public List<MonsterSO> monsters;
    public List<Monster> midBossMonsters;

    public float enemiesPerBoss = 20f; // Ȯ�强�� ���� �� óġ �� ����
    public int TotalSliderSteps = 10; // �����̴� �ܰ� �� (���� ��ȯ ����)

    public int currentStageIndex = 0;
    public float enemyKillCount = 0f;
    public float sliderEnemyCount = 0f;

    public float stageSliderValue; // �����̴� ���� ������ ���� �߰�

    public int midBossIndex = 0; // �߰� ���� ���� ���� �ε���

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