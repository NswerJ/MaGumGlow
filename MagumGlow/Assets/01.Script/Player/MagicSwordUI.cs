using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSwordUI : MonoBehaviour
{
    public MagicSwordStats magicSwordStats;  // ���� ���� SO ����
    public Button attackPowerButton;  // ���ݷ� ���� ��ư
    public Button healthButton;  // ���� ���� ��ư
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
    }

    void LevelUpStat(string statName)
    {
        magicSwordStats.LevelUpStat(statName);
        UpdateUI(statName);  // UI ������Ʈ �Լ�
    }

    void UpdateUI(string statName)
    {
        Debug.Log($"{statName} ���� UI�� ���ŵǾ����ϴ�.");
    }
}
