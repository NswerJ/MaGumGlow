using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawController : MonoBehaviour
{
    public MagicSwordStats stats; 
    public float drawGold;
    public TextMeshProUGUI playerGoldTxt;
    public Button drawButton;

    private enum JewelGrade { Normal, Rare, Legendary }

    public void DrawJewelButton()
    {
        DrawJewel();
        /*if (stats.playerGold >= drawGold)
        {
           
            stats.playerGold -= drawGold; 
        }
        else
        {
            Debug.Log("°ñµå ¾øÀ½.");
        }*/
    }

    private void Update()
    {
        UpdateGoldUI();

    }

    private void UpdateGoldUI()
    {
        playerGoldTxt.text = stats.playerGold.ToString();
    }

    private void DrawJewel()
    {
        float randomNum = Random.Range(0f, 100f); 

        JewelGrade drawnJewel = JewelGrade.Normal; 

        if (randomNum <= 70f)
        {
            drawnJewel = JewelGrade.Normal;
            NormalWinning();
        }
        else if (randomNum <= 99.5f)
        {
            drawnJewel = JewelGrade.Rare;
            RareWinning();
        }
        else
        {
            drawnJewel = JewelGrade.Legendary;
            LegendaryWinning();
        }

        Debug.Log($"º¸¼® »Ì±â °á°ú: {drawnJewel}");
    }

    public void NormalWinning()
    {
        ButtonInteractable(false);
    }

    public void RareWinning()
    {
        ButtonInteractable(false);
    }

    public void LegendaryWinning()
    {
        ButtonInteractable(false);
    }

    public void ButtonInteractable(bool isInteractable)
    {
        drawButton.interactable = isInteractable;
    }
}
