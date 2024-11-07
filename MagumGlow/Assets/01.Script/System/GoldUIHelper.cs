using TMPro;

public static class GoldUIHelper
{
    public static void UpdateGoldUI(TMP_Text playerGoldText, float playerGold)
    {
        playerGoldText.text = $"���: {NumberFormatter.FormatWithUnit(playerGold)}";
    }
}
