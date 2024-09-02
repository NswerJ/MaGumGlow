using TMPro;
using UnityEngine;

public class MagicSword : MonoBehaviour
{
    public MagicSwordStats swordStats;  // 마검 스탯 SO 참조
    public TextMeshProUGUI playerName;

    void Start()
    {
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
