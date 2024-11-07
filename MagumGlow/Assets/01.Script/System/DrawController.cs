using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawController : MonoBehaviour
{
    public MagicSwordStats stats;
    public float drawGold;
    public TextMeshProUGUI playerGoldTxt;
    public TextMeshProUGUI jewelPowerTxt;
    public Button drawButton;
    public DrawAnimationController animationController;

    [SerializeField] private JewelResultProcessor resultProcessor;  

    public ButtonController buttonController;

    public enum JewelGrade { Normal, Rare, Legendary }

    private void Start()
    {
        if (resultProcessor == null)
        {
            resultProcessor = new JewelResultProcessor(stats);
        }
    }

    private void Update()
    {
        GoldUIHelper.UpdateGoldUI(playerGoldTxt, stats.playerGold);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animationController.ShowDrawPanel(true);
        }
    }

    public void DrawJewelButton()
    {
        if (stats.playerGold >= drawGold)
        {
            stats.playerGold -= drawGold;
            StartCoroutine(HandleDrawJewel());
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    private IEnumerator HandleDrawJewel()
    {
        buttonController.SetButtonInteractable(drawButton, false);

        JewelGrade drawnJewel = DrawJewel();

        yield return new WaitForSeconds(2.0f);

        resultProcessor.ProcessJewelResult(drawnJewel);  // Null이 아니라면 결과 처리

        buttonController.SetButtonInteractable(drawButton, true);
    }

    private JewelGrade DrawJewel()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum <= 70f)
            return JewelGrade.Normal;
        else if (randomNum <= 99.5f)
            return JewelGrade.Rare;
        else
            return JewelGrade.Legendary;
    }
}


public class JewelResultProcessor
{
    private MagicSwordStats stats;

    public JewelResultProcessor(MagicSwordStats stats)
    {
        this.stats = stats;
    }

    public void ProcessJewelResult(DrawController.JewelGrade grade)
    {
        float multiplier = grade switch
        {
            DrawController.JewelGrade.Legendary => 2f,
            DrawController.JewelGrade.Rare => 1f,
            DrawController.JewelGrade.Normal => 0.5f,
            _ => 1f
        };

        if (stats.stats.Count > 0)
        {
            int randomIndex = Random.Range(0, stats.stats.Count);
            Stat selectedStat = stats.stats[randomIndex];

            selectedStat.currentValue += selectedStat.currentValue * multiplier;

            Debug.Log($"{grade} 등급 보석 획득으로 {selectedStat.statName} 스탯이 {multiplier}배 증가했습니다.");
        }
        else
        {
            Debug.LogWarning("스탯 목록이 비어있습니다.");
        }
    }

}
