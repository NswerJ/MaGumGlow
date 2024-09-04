using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicSwordStats", menuName = "SO/MagicSwordStats")]
public class MagicSwordStats : ScriptableObject
{
    public List<Stat> stats;  // ������ �پ��� ���� ���
    public string playerName;  // �÷��̾� ���� �̸�

    public void LevelUpStat(string statName)
    {
        // ���� �̸����� ����Ʈ���� �ش� ������ ã��
        Stat targetStat = stats.Find(stat => stat.statName == statName);

        if (targetStat != null)
        {
            if (targetStat.currentValue < targetStat.maxValue)
            {

                targetStat.currentValue = Mathf.Min(targetStat.currentValue + targetStat.baseValue * (targetStat.statLevel + 1), targetStat.maxValue);
                targetStat.statLevel++;
                Debug.Log($"{statName} ������ ������ �Ǿ����ϴ�. ���ο� ��: {targetStat.currentValue}");
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
}
