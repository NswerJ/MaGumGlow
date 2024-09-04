using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;  // 마검 스탯 SO 참조
    public Button attackPowerButton;  // 공격력 증가 버튼
    public Button healthButton;  // 방어력 증가 버튼
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
    }

    void LevelUpStat(string statName)
    {
        magicSwordStats.LevelUpStat(statName);
        UpdateUI(statName);  // UI 업데이트 함수
    }

    void UpdateUI(string statName)
    {
        Debug.Log($"{statName} 스탯 UI가 갱신되었습니다.");
    }
}
