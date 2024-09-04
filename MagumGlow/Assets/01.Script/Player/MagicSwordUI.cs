using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;  // 마검 스탯 SO 참조
    public Button attackPowerButton;  // 공격력 증가 버튼
    public Button healthButton;  // 생명력 증가 버튼
    public TMP_Text playerNameText; // 플레이어 이름을 표시할 텍스트 필드

    void Start()
    {
        // 저장된 플레이어 이름을 가져옴
        magicSwordStats.playerName = PlayerPrefs.GetString("playerName", "Unknown Player");

        // UI에 플레이어 이름 표시
        playerNameText.text = magicSwordStats.playerName;

        // 각 버튼에 클릭 이벤트 할당
        attackPowerButton.onClick.AddListener(() => LevelUpStat("공격력"));
        healthButton.onClick.AddListener(() => LevelUpStat("생명력"));
        UpdateUI("공격력");
       // UpdateUI("생명력");
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
            Debug.Log(statParent);
        }
        else if (statName == "생명력")
        {
            statParent = healthButton.transform.parent;
            Debug.Log(statParent);
        }

        if (statParent != null)
        {
            TMP_Text levelStatText = statParent.Find("LevelStat").GetComponent<TMP_Text>();
            TMP_Text increaseStatText = statParent.Find("IncreaseStat").GetComponent<TMP_Text>();

            // 현재 스탯 정보 가져오기
            Stat stat = magicSwordStats.stats.Find(s => s.statName == statName);

            if (stat != null)
            {
                // 스탯의 레벨과 증가된 값 표시
                levelStatText.text = $"Level: {stat.statLevel}";
                increaseStatText.text = $"Damage: {stat.currentValue}";
            }

            Debug.Log($"{statName} 스탯 UI가 갱신되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{statName}에 대한 부모 오브젝트를 찾을 수 없습니다.");
        }
    }
}
