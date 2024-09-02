using System;
using TMPro;
using UnityEngine;

public class MagicSword : MonoBehaviour
{
    public MagicSwordStats swordStats;  // 마검 스탯 SO 참조
    public TextMeshProUGUI playerName;
    public bool enemyIsFront;

    public event Action<bool> AttackEvent;

    void Start()
    {
        enemyIsFront = false;
        InitializeSword();
    }

    void InitializeSword()
    {
        playerName.text = swordStats.playerName;
        // 마검 초기화 시, 스탯을 기본 값으로 설정
        foreach (var stat in swordStats.stats)
        {
            stat.currentValue = stat.baseValue;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            enemyIsFront = true;
        }else if (Input.GetKeyDown(KeyCode.Q))
        {
            enemyIsFront = false;
        }

        if (enemyIsFront)
        {
            Attacking(true);
        }
        else
        {
            Attacking(false);
        }
    }

    private void Attacking(bool isAttacking)
    {
        AttackEvent?.Invoke(isAttacking);
    }

    public void LevelUpSword()
    {
        swordStats.LevelUp();
        // 이후 마검의 스탯을 UI 등에 반영
    }

    public void DisplaySwordStats()
    {
        // 마검의 현재 스탯을 UI 등에 표시하는 함수
        foreach (var stat in swordStats.stats)
        {
            Debug.Log($"{stat.statName}: {stat.currentValue}");
        }
    }
}
