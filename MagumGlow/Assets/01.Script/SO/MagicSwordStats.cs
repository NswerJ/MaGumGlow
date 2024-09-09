using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicSwordStats", menuName = "SO/MagicSwordStats")]
public class MagicSwordStats : ScriptableObject
{
    public List<Stat> stats;  // 마검의 다양한 스탯 목록
    public string playerName;  // 플레이어 설정 이름

    public void LevelUpStat(string statName)
    {
        Stat targetStat = stats.Find(stat => stat.statName == statName);

        if (targetStat != null)
        {
            if (targetStat.currentValue < targetStat.maxValue)
            {
                targetStat.currentValue = Mathf.Min(targetStat.currentValue + targetStat.baseValue * (targetStat.statLevel + 1), targetStat.maxValue);
                targetStat.statLevel++;

                Debug.Log($"{statName} 스탯이 레벨업 되었습니다. 새로운 값: {targetStat.currentValue.ToString("N0")}");
            }
            else
            {
                Debug.LogWarning($"{statName} 스탯이 이미 최대값({targetStat.maxValue})에 도달했습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"{statName}이라는 이름의 스탯이 없습니다.");
        }
    }

    // 모든 스탯을 초기화하는 메서드
    public void ResetStats()
    {
        foreach (var stat in stats)
        {
            stat.statLevel = 1;
            stat.currentValue = stat.baseValue;
        }
        Debug.Log("모든 스탯이 초기화되었습니다.");
    }
}
