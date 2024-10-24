using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawController : MonoBehaviour
{
    public MagicSwordStats stats;
    public float drawGold;
    public TextMeshProUGUI playerGoldTxt;
    public Button drawButton;
    public DrawAnimationController animationController;

    [SerializeField] private JewelResultProcessor resultProcessor;  // �ν����Ϳ��� �Ҵ�

    public ButtonController buttonController;

    public enum JewelGrade { Normal, Rare, Legendary }

    private void Start()
    {
        if (resultProcessor == null)
        {
            resultProcessor = new JewelResultProcessor();  // Null üũ �� �ν��Ͻ�ȭ
        }
    }

    private void Update()
    {
        GoldUIHelper.UpdateGoldUI(playerGoldTxt, stats.playerGold);
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
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    private IEnumerator HandleDrawJewel()
    {
        buttonController.SetButtonInteractable(drawButton, false);

        JewelGrade drawnJewel = DrawJewel();

        yield return new WaitForSeconds(2.0f);

        resultProcessor.ProcessJewelResult(drawnJewel);  // Null�� �ƴ϶�� ��� ó��

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
    public void ProcessJewelResult(DrawController.JewelGrade grade)
    {
        switch (grade)
        {
            case DrawController.JewelGrade.Normal:
                Debug.Log("�븻 ��� ����");
                break;
            case DrawController.JewelGrade.Rare:
                Debug.Log("���� ��� ����");
                break;
            case DrawController.JewelGrade.Legendary:
                Debug.Log("���� ��� ����");
                break;
        }
    }
}
