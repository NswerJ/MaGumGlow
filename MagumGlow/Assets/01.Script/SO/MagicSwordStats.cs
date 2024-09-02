using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicSwordStats", menuName = "SO/MagicSwordStats")]
public class MagicSwordStats : ScriptableObject
{
    public List<Stat> stats;  // ������ �پ��� ���� ���
    public int level;  // ���� ������ ����
    public int maxLevel;  // �ִ� ����
    public string playerName;  // �÷��̾� ���� �̸�

    public void LevelUp()
    {
        if (level < maxLevel)
        {
            level++;
            foreach (var stat in stats)
            {
                stat.currentValue = stat.baseValue * level;  // ������ ���� ���� ���� (������ ����)
            }
        }
        else
        {
            Debug.Log("������ �̹� �ִ� ������ �����߽��ϴ�.");
        }
    }
}
