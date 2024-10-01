using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordStatUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;
    public Button attackPowerButton;
    public Button healthButton;
    public Button criticalPowerButton;
    public Button criticalPercentButton;
    public TMP_Text playerNameText;
    public TMP_Text playerGoldText;
    public Slider playerHealthSlider;
    public List<string> statNameList;

    private const string AttackPower = "공격력";
    private const string Health = "생명력";
    private const string CriticalPower = "치명타데미지";
    private const string CriticalPercent = "치명타확률";

    void Start()
    {
        // Load the player data from JSON
        GameManager.Instance.LoadPlayerData();

        magicSwordStats.GetGold += UpdateGoldUI;

        SetPlayerName();
        AssignButtonListeners();
        InitializeUI();
    }

    void SetPlayerName()
    {
        playerNameText.text = magicSwordStats.playerName;
    }

    void AssignButtonListeners()
    {
        attackPowerButton.onClick.AddListener(() => LevelUpStat(AttackPower));
        healthButton.onClick.AddListener(() => LevelUpStat(Health));
        criticalPowerButton.onClick.AddListener(() => LevelUpStat(CriticalPower));
        criticalPercentButton.onClick.AddListener(() => LevelUpStat(CriticalPercent));
    }

    void InitializeUI()
    {
        foreach (string statName in statNameList)
        {
            UpdateUI(statName);
            UpdateCostUI(statName);
        }
        UpdateGoldUI();
    }

    void LevelUpStat(string statName)
    {
        magicSwordStats.LevelUpStat(statName);
        UpdateUI(statName);
        UpdateCostUI(statName);
        UpdateGoldUI();

        // Save player data after stat level up
        GameManager.Instance.SavePlayerData();
    }

    void UpdateCostUI(string statName)
    {
        Transform statParent = GetStatParent(statName);
        if (statParent == null)
        {
            Debug.LogWarning($"{statName}에 대한 부모 오브젝트를 찾을 수 없습니다.");
            return;
        }

        TMP_Text costText = statParent.Find("Goldtext").GetComponent<TMP_Text>();
        Stat stat = GetStat(statName);

        if (stat != null)
        {
            costText.text = $"필요 골드 : {NumberFormatter.FormatWithUnit(stat.levelUpCost)}";
        }
    }

    void UpdateUI(string statName)
    {
        Transform statParent = GetStatParent(statName);
        if (statParent == null)
        {
            Debug.LogWarning($"{statName}에 대한 부모 오브젝트를 찾을 수 없습니다.");
            return;
        }

        TMP_Text levelStatText = statParent.Find("LevelStat").GetComponent<TMP_Text>();
        TMP_Text increaseStatText = statParent.Find("IncreaseStat").GetComponent<TMP_Text>();

        Stat stat = GetStat(statName);
        if (stat != null)
        {
            levelStatText.text = $"레벨: {stat.statLevel}";
            increaseStatText.text = $"수치: {NumberFormatter.FormatWithUnit(stat.currentValue)}";

            if (statName == Health)
            {
                UpdateHealthSlider(stat.currentValue);
            }
        }
    }

    void UpdateGoldUI()
    {
        playerGoldText.text = $"골드: {NumberFormatter.FormatWithUnit(magicSwordStats.playerGold)}";
    }

    Transform GetStatParent(string statName)
    {
        return statName switch
        {
            AttackPower => attackPowerButton.transform.parent,
            Health => healthButton.transform.parent,
            CriticalPower => criticalPowerButton.transform.parent,
            CriticalPercent => criticalPercentButton.transform.parent,
            _ => null
        };
    }

    Stat GetStat(string statName)
    {
        return magicSwordStats.stats.Find(s => s.statName == statName);
    }

    void UpdateHealthSlider(float currentValue)
    {
        playerHealthSlider.maxValue = currentValue;
        playerHealthSlider.value = currentValue;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
