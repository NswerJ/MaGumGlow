using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;  // ���� ���� SO ����
    public Button attackPowerButton;  // ���ݷ� ���� ��ư
    public Button healthButton;  // ����� ���� ��ư
    public TMP_Text playerNameText; // �÷��̾� �̸��� ǥ���� �ؽ�Ʈ �ʵ�

    void Start()
    {
        // ����� �÷��̾� �̸��� ������
        magicSwordStats.playerName = PlayerPrefs.GetString("playerName", "Unknown Player");

        // UI�� �÷��̾� �̸� ǥ��
        playerNameText.text = magicSwordStats.playerName;

        // �� ��ư�� Ŭ�� �̺�Ʈ �Ҵ�
        attackPowerButton.onClick.AddListener(() => LevelUpStat("���ݷ�"));
        healthButton.onClick.AddListener(() => LevelUpStat("�����"));
        UpdateUI("���ݷ�");
       // UpdateUI("�����");
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
            Debug.Log(statParent);
        }
        else if (statName == "�����")
        {
            statParent = healthButton.transform.parent;
            Debug.Log(statParent);
        }

        if (statParent != null)
        {
            TMP_Text levelStatText = statParent.Find("LevelStat").GetComponent<TMP_Text>();
            TMP_Text increaseStatText = statParent.Find("IncreaseStat").GetComponent<TMP_Text>();

            // ���� ���� ���� ��������
            Stat stat = magicSwordStats.stats.Find(s => s.statName == statName);

            if (stat != null)
            {
                // ������ ������ ������ �� ǥ��
                levelStatText.text = $"Level: {stat.statLevel}";
                increaseStatText.text = $"Damage: {stat.currentValue}";
            }

            Debug.Log($"{statName} ���� UI�� ���ŵǾ����ϴ�.");
        }
        else
        {
            Debug.LogWarning($"{statName}�� ���� �θ� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
