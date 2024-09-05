using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;  // 마검 스탯 SO 참조
    public Button attackPowerButton;  // 공격력 증가 버튼
    public Button healthButton;  // 생명력 증가 버튼
    public Button criticalPowerhButton;  // 치명타 데미지 증가 버튼
    public Button criticalPercenthButton;  // 치명타 확률 증가 버튼
    public TMP_Text playerNameText; // 플레이어 이름을 표시할 텍스트 필드

    void Start()
    {
        magicSwordStats.playerName = PlayerPrefs.GetString("playerName", "Unknown Player");

        playerNameText.text = magicSwordStats.playerName;

        attackPowerButton.onClick.AddListener(() => LevelUpStat("공격력"));
        healthButton.onClick.AddListener(() => LevelUpStat("생명력"));
        criticalPowerhButton.onClick.AddListener(() => LevelUpStat("치명타데미지"));
        criticalPercenthButton.onClick.AddListener(() => LevelUpStat("치명타확률"));

        UpdateUI("공격력");
        UpdateUI("생명력");
        UpdateUI("치명타데미지");
        UpdateUI("치명타확률");
    }

    void LevelUpStat(string statName)
    {
        magicSwordStats.LevelUpStat(statName);
        UpdateUI(statName);
    }

    void UpdateUI(string statName)
    {
        Transform statParent = null;

        if (statName == "공격력")
        {
            statParent = attackPowerButton.transform.parent;
        }
        else if (statName == "생명력")
        {
            statParent = healthButton.transform.parent;
        }
        else if (statName == "치명타데미지")
        {
            statParent = criticalPowerhButton.transform.parent;
        }
        else if (statName == "치명타확률")
        {
            statParent = criticalPercenthButton.transform.parent;
        }

        if (statParent != null)
        {
            TMP_Text levelStatText = statParent.Find("LevelStat").GetComponent<TMP_Text>();
            TMP_Text increaseStatText = statParent.Find("IncreaseStat").GetComponent<TMP_Text>();

            Stat stat = magicSwordStats.stats.Find(s => s.statName == statName);

            if (stat != null)
            {
                levelStatText.text = $"레벨: {stat.statLevel}";
                increaseStatText.text = $"수치: {NumberFormatter.FormatWithUnit(stat.currentValue)}";
            }

            Debug.Log($"{statName} 스탯 UI가 갱신되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{statName}에 대한 부모 오브젝트를 찾을 수 없습니다.");
        }
    }

}
