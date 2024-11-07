using TMPro;

public static class GoldUIHelper
{
    public static void UpdateGoldUI(TMP_Text playerGoldText, float playerGold)
    {
        playerGoldText.text = $"°ñµå: {NumberFormatter.FormatWithUnit(playerGold)}";
    }
}
