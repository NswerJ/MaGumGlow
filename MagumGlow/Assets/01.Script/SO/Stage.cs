using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Stage")]
public class Stage : ScriptableObject
{
    public string StageName;
    public List<MonsterSO> monsters;
    public List<Monster> midBossMonsters;

    public float enemiesPerBoss = 20f; // 확장성을 위해 적 처치 수 설정
    public int TotalSliderSteps = 10; // 슬라이더 단계 수 (보스 소환 구간)

    public int currentStageIndex = 0;
    public float enemyKillCount = 0f;
    public float sliderEnemyCount = 0f;

    public float stageSliderValue; // 슬라이더 값을 저장할 변수 추가

    public int midBossIndex = 0; // 중간 보스 순서 관리 인덱스

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