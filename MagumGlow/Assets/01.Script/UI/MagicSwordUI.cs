using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;  // ���� ���� SO ����
    public Button attackPowerButton;  // ���ݷ� ���� ��ư
    public Button healthButton;  // ����� ���� ��ư
    public Button criticalPowerButton;  // ġ��Ÿ ������ ���� ��ư
    public Button criticalPercentButton;  // ġ��Ÿ Ȯ�� ���� ��ư
    public TMP_Text playerNameText;  // �÷��̾� �̸��� ǥ���� �ؽ�Ʈ �ʵ�
    public TMP_Text playerGoldText;   // �÷��̾��� ��带 ǥ���� �ؽ�Ʈ �ʵ�
    public Slider playerHealthSlider; // �÷��̾� ü�� Slider

    void Start()
    {
        SetPlayerName();
        AssignButtonListeners();
        InitializeUI();
    }

    void SetPlayerName()
    {
        magicSwordStats.playerName = PlayerPrefs.GetString("playerName", "Unknown Player");
        playerNameText.text = magicSwordStats.playerName;
    }

    void AssignButtonListeners()
    {
        attackPowerButton.onClick.AddListener(() => LevelUpStat("���ݷ�"));
        healthButton.onClick.AddListener(() => LevelUpStat("�����"));
        criticalPowerButton.onClick.AddListener(() => LevelUpStat("ġ��Ÿ������"));
        criticalPercentButton.onClick.AddListener(() => LevelUpStat("ġ��ŸȮ��"));
    }

    void InitializeUI()
    {
        UpdateUI("���ݷ�");
        UpdateUI("�����");
        UpdateUI("ġ��Ÿ������");
        UpdateUI("ġ��ŸȮ��");
        UpdateCostUI("���ݷ�");
        UpdateCostUI("�����");
        UpdateCostUI("ġ��Ÿ������");
        UpdateCostUI("ġ��ŸȮ��");
        UpdateGoldUI();  // ��� ǥ�� ������Ʈ
    }

    void LevelUpStat(string statName)
    {
        magicSwordStats.LevelUpStat(statName);
        UpdateUI(statName);
        UpdateCostUI(statName);
        // ������ �� ��� UI ������Ʈ
    }
    private void Update()
    {
        UpdateGoldUI();
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
        Stat stat = magicSwordStats.stats.Find(s => s.statName == statName);

        if(stat != null) {
            costText.text = $"�ʿ� ��� : {NumberFormatter.FormatWithUnit(stat.levelUpCost)}";
        }

        Debug.Log($"{statName} ���� �ڽ�Ʈ UI�� ���ŵǾ����ϴ�.");
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

        Stat stat = magicSwordStats.stats.Find(s => s.statName == statName);
        if (stat != null)
        {
            levelStatText.text = $"����: {stat.statLevel}";
            increaseStatText.text = $"��ġ: {NumberFormatter.FormatWithUnit(stat.currentValue)}";

            if (statName == "�����")
            {
                UpdateHealthSlider(stat.currentValue);
            }
        }

        Debug.Log($"{statName} ���� UI�� ���ŵǾ����ϴ�.");
    }

    void UpdateGoldUI()
    {
        playerGoldText.text = $"���: {NumberFormatter.FormatWithUnit(magicSwordStats.playerGold)}";
    }

    Transform GetStatParent(string statName)
    {
        return statName switch
        {
            "���ݷ�" => attackPowerButton.transform.parent,
            "�����" => healthButton.transform.parent,
            "ġ��Ÿ������" => criticalPowerButton.transform.parent,
            "ġ��ŸȮ��" => criticalPercentButton.transform.parent,
            _ => null
        };
    }

    void UpdateHealthSlider(float currentValue)
    {
        playerHealthSlider.maxValue = currentValue;
        playerHealthSlider.value = currentValue;
    }
}
