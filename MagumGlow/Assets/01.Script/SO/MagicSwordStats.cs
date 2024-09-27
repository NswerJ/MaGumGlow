using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicSwordStats", menuName = "SO/MagicSwordStats")]
public class MagicSwordStats : ScriptableObject
{
    public List<Stat> stats;  // ������ �پ��� ���� ���
    public string playerName;  // �÷��̾� ���� �̸�
    public float playerGold;  // �÷��̾ ������ ���

    public event Action GetGold;

    public void LevelUpStat(string statName)
    {
        Stat targetStat = stats.Find(stat => stat.statName == statName);

        if (targetStat != null)
        {
            if (targetStat.currentValue < targetStat.maxValue)
            {
                if (playerGold >= targetStat.levelUpCost)
                {
                    playerGold -= targetStat.levelUpCost; 
                    targetStat.currentValue = Mathf.Min(targetStat.currentValue + targetStat.baseValue * (targetStat.statLevel + 1), targetStat.maxValue);
                    targetStat.statLevel++;

                    targetStat.levelUpCost *= 1.2f;  

                    Debug.Log($"{statName} ������ ������ �Ǿ����ϴ�. ���ο� ��: {targetStat.currentValue}, ���� ���: {playerGold}");
                }
                else
                {
                    Debug.LogWarning("��尡 �����մϴ�.");
                }
            }
            else
            {
                Debug.LogWarning($"{statName} ������ �̹� �ִ밪({targetStat.maxValue})�� �����߽��ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning($"{statName}�̶�� �̸��� ������ �����ϴ�.");
        }
    }

    public void AddGold(float amount)
    {
        playerGold += amount;
        Debug.Log($"�÷��̾ {amount} ��带 ������ϴ�. ���� ���: {playerGold}");
        GetGold?.Invoke();
    }

    // ��� ������ �ʱ�ȭ�ϴ� �޼���
    public void ResetStats()
    {
        foreach (var stat in stats)
        {
            stat.statLevel = 1;
            stat.currentValue = stat.baseValue;
            stat.levelUpCost = stat.baseCost;
            playerGold = 0;
        }
        Debug.Log("��� ������ �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
