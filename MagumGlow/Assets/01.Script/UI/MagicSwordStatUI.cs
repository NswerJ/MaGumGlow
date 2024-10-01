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

    private const string AttackPower = "���ݷ�";
    private const string Health = "�����";
    private const string CriticalPower = "ġ��Ÿ������";
    private const string CriticalPercent = "ġ��ŸȮ��";

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
            Debug.LogWarning($"{statName}�� ���� �θ� ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        TMP_Text costText = statParent.Find("Goldtext").GetComponent<TMP_Text>();
        Stat stat = GetStat(statName);

        if (stat != null)
        {
            costText.text = $"�ʿ� ��� : {NumberFormatter.FormatWithUnit(stat.levelUpCost)}";
        }
    }

    void UpdateUI(string statName)
    {
        Transform statParent = GetStatParent(statName);
        if (statParent == null)
        {
            Debug.LogWarning($"{statName}�� ���� �θ� ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        TMP_Text levelStatText = statParent.Find("LevelStat").GetComponent<TMP_Text>();
        TMP_Text increaseStatText = statParent.Find("IncreaseStat").GetComponent<TMP_Text>();

        Stat stat = GetStat(statName);
        if (stat != null)
        {
            levelStatText.text = $"����: {stat.statLevel}";
            increaseStatText.text = $"��ġ: {NumberFormatter.FormatWithUnit(stat.currentValue)}";

            if (statName == Health)
            {
                UpdateHealthSlider(stat.currentValue);
            }
        }
    }

    void UpdateGoldUI()
    {
        playerGoldText.text = $"���: {NumberFormatter.FormatWithUnit(magicSwordStats.playerGold)}";
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
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
