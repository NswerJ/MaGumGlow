using TMPro;
using UnityEngine;

public class MagicSword : MonoBehaviour
{
    public MagicSwordStats swordStats;  // ���� ���� SO ����
    public TextMeshProUGUI playerName;

    void Start()
    {
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
