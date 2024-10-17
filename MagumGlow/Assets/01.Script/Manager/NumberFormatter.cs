using UnityEngine;

public static class NumberFormatter
{
    // 숫자에 맞는 단위를 붙여주는 메서드
    public static string FormatWithUnit(float number)
    {
        if (number >= 1e12f)  // 조
        {
            return (number / 1e12f).ToString("F1") + "조";
        }
        else if (number >= 1e8f)  // 억
        {
            return (number / 1e8f).ToString("F1") + "억";
        }
        else if (number >= 1e4f)  // 만
        {
            return (number / 1e4f).ToString("F1") + "만";
        }
        else
        {
            return number.ToString("N0");  // 천 단위 구분만 적용
        }
    }
}
