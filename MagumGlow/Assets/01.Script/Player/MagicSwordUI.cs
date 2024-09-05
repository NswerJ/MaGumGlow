using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;  // ���� ���� SO ����
    public Button attackPowerButton;  // ���ݷ� ���� ��ư
    public Button healthButton;  // ����� ���� ��ư
    public Button criticalPowerhButton;  // ġ��Ÿ ������ ���� ��ư
    public Button criticalPercenthButton;  // ġ��Ÿ Ȯ�� ���� ��ư
    public TMP_Text playerNameText; // �÷��̾� �̸��� ǥ���� �ؽ�Ʈ �ʵ�

    void Start()
    {
        magicSwordStats.playerName = PlayerPrefs.GetString("playerName", "Unknown Player");

        playerNameText.text = magicSwordStats.playerName;

        attackPowerButton.onClick.AddListener(() => LevelUpStat("���ݷ�"));
        healthButton.onClick.AddListener(() => LevelUpStat("�����"));
        criticalPowerhButton.onClick.AddListener(() => LevelUpStat("ġ��Ÿ������"));
        criticalPercenthButton.onClick.AddListener(() => LevelUpStat("ġ��ŸȮ��"));

        UpdateUI("���ݷ�");
        UpdateUI("�����");
        UpdateUI("ġ��Ÿ������");
        UpdateUI("ġ��ŸȮ��");
    }

    void LevelUpStat(string statName)
    {
        magicSwordStats.LevelUpStat(statName);
        UpdateUI(statName);
    }

    void UpdateUI(string statName)
    {
        Transform statParent = null;

        if (statName == "���ݷ�")
        {
            statParent = attackPowerButton.transform.parent;
        }
        else if (statName == "�����")
        {
            statParent = healthButton.transform.parent;
        }
        else if (statName == "ġ��Ÿ������")
        {
            statParent = criticalPowerhButton.transform.parent;
        }
        else if (statName == "ġ��ŸȮ��")
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
                levelStatText.text = $"����: {stat.statLevel}";
                increaseStatText.text = $"��ġ: {NumberFormatter.FormatWithUnit(stat.currentValue)}";
            }

            Debug.Log($"{statName} ���� UI�� ���ŵǾ����ϴ�.");
        }
        else
        {
            Debug.LogWarning($"{statName}�� ���� �θ� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

}
