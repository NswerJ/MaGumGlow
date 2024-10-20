using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    public MagicSwordStats stats; 
    public float drawGold;

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

    private void DrawJewel()
    {
        float randomNum = Random.Range(0f, 100f); 

        JewelGrade drawnJewel = JewelGrade.Normal; 

        if (randomNum <= 70f)
        {
            drawnJewel = JewelGrade.Normal; 
        }
        else if (randomNum <= 99.5f)
        {
            drawnJewel = JewelGrade.Rare; 
        }
        else
        {
            drawnJewel = JewelGrade.Legendary; 
        }

        Debug.Log($"º¸¼® »Ì±â °á°ú: {drawnJewel}");
    }
}
