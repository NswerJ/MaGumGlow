using UnityEngine;

public class GameController : MonoBehaviour
{
    public MagicSword magicSword;  // ���ӿ� ��ġ�� ���� ����

    void Update()
    {
        // ����: ���� �ð� ������� ���� ������
        if (ShouldLevelUp())
        {
            magicSword.LevelUpSword();
            magicSword.DisplaySwordStats();
        }
    }

    bool ShouldLevelUp()
    {
        // ������ ���� ���� (��: ���� �ð� ���, ����ġ ȹ�� ��)
        return Time.time % 10 == 0;  // ���÷� 10�ʸ��� ������
    }
}
