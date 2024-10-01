using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInputField;  // 플레이어 이름 입력 필드
    public GameObject playerNameButton;  // 플레이어 이름 입력 필드 부모
    public GameObject startButton;  // 플레이어 이름 입력 필드 부모
    public GameObject orginstartButton;  // 플레이어 이름 입력 필드 부모
    public TMP_Text warningText;  // 경고 메시지를 표시할 텍스트
    private Coroutine fadeCoroutine;  // 현재 실행 중인 페이드 아웃 코루틴을 관리하기 위한 참조

    private void Awake()
    {
        playerNameInputField.characterLimit = 10;  // 입력 글자수 제한 (10글자)
        startButton.SetActive(false);
        orginstartButton.SetActive(false);
        warningText.gameObject.SetActive(false);

        // 이미 저장된 플레이어 이름이 있는지 확인
        if (!string.IsNullOrEmpty(GameManager.Instance.playerData.playerName))
        {
            EnableSceneChangeButton();
            Debug.Log("이름이 있음");
        }
        else
        {
            EnableNameInput();
            Debug.Log("이름 써야함");
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
            // 플레이어 이름을 저장하고 씬을 변경
            GameManager.Instance.SetPlayerName(playerNameInputField.text);
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OriginGameSceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
