using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInputField;  
    public GameObject playerNameButton;  
    public GameObject startButton;  
    public GameObject orginstartButton;  
    public TMP_Text warningText;  
    private Coroutine fadeCoroutine;  

    private void Awake()
    {
        playerNameInputField.characterLimit = 10;  
        startButton.SetActive(false);
        orginstartButton.SetActive(false);
        warningText.gameObject.SetActive(false);
        var curStageData = GameManager.Instance.curStageData; 
        if (!string.IsNullOrEmpty(GameManager.Instance.playerData.playerName))
        {
            EnableSceneChangeButton();
            Debug.Log("이름이 있음");
        }
        else
        {
            curStageData.stageSliderValue = 0;
            EnableNameInput();
            Debug.Log("이름 써야함");
            Debug.Log("슬라이더 초기화");

        }
    }

    public void EnableNameInput()
    {
        playerNameButton.SetActive(true);
        orginstartButton.SetActive(false);
    }

    public void EnableSceneChangeButton()
    {
        playerNameButton.SetActive(false);
        orginstartButton.SetActive(true);
    }

    public void NewGameSceneChange(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(playerNameInputField.text))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 1);
            warningText.text = "이름을 입력하세요!";
            warningText.gameObject.SetActive(true);

            fadeCoroutine = StartCoroutine(FadeOutWarningText());
        }
        else
        {
            GameManager.Instance.SetPlayerName(playerNameInputField.text);
            LoadingSceneController.Instance.LoadScene(sceneName);
        }
    }

    public void OriginGameSceneChange(string sceneName)
    {
        LoadingSceneController.Instance.LoadScene(sceneName);
    }

    private IEnumerator FadeOutWarningText()
    {
        float duration = 2.0f;
        float elapsedTime = 0f;

        Color originalColor = warningText.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            warningText.color = Color.Lerp(originalColor, transparentColor, elapsedTime / duration);
            yield return null;
        }

        warningText.gameObject.SetActive(false);
    }
}
