using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicSwordStats", menuName = "SO/MagicSwordStats")]
public class MagicSwordStats : ScriptableObject
{
    public List<Stat> stats;  // 마검의 다양한 스탯 목록
    public int level;  // 현재 마검의 레벨
    public int maxLevel;  // 최대 레벨
    public string playerName;  // 플레이어 설정 이름

    public void LevelUp()
    {
        if (level < maxLevel)
        {
            level++;
            foreach (var stat in stats)
            {
                stat.currentValue = stat.baseValue * level;  // 레벨에 따라 스탯 증가 (간단한 예시)
            }
        }
        else
        {
            Debug.Log("마검이 이미 최대 레벨에 도달했습니다.");
        }
    }
}
