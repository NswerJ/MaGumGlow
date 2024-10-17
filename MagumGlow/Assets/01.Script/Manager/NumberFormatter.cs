using UnityEngine;

public static class NumberFormatter
{
    // ���ڿ� �´� ������ �ٿ��ִ� �޼���
    public static string FormatWithUnit(float number)
    {
        if (number >= 1e12f)  // ��
        {
            return (number / 1e12f).ToString("F1") + "��";
        }
        else if (number >= 1e8f)  // ��
        {
            return (number / 1e8f).ToString("F1") + "��";
        }
        else if (number >= 1e4f)  // ��
        {
            return (number / 1e4f).ToString("F1") + "��";
        }
        else
        {
            return number.ToString("N0");  // õ ���� ���и� ����
        }
    }
}
