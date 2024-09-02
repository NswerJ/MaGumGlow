using System;
using TMPro;
using UnityEngine;

public class MagicSword : MonoBehaviour
{
    public MagicSwordStats swordStats;  // ���� ���� SO ����
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
        // ���� �ʱ�ȭ ��, ������ �⺻ ������ ����
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
        // ���� ������ ������ UI � �ݿ�
    }

    public void DisplaySwordStats()
    {
        // ������ ���� ������ UI � ǥ���ϴ� �Լ�
        foreach (var stat in swordStats.stats)
        {
            Debug.Log($"{stat.statName}: {stat.currentValue}");
        }
    }
}
