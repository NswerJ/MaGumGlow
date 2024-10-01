using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInputField;  // 플레이어 이름 입력 필드
    public GameObject playerNameButton;  // 플레이어 이름 입력 필드 부모
    public TMP_Text warningText;  // 경고 메시지를 표시할 텍스트
    private Coroutine fadeCoroutine;  // 현재 실행 중인 페이드 아웃 코루틴을 관리하기 위한 참조
    private bool isNameEntered = false; // 이름이 입력되었는지 여부 확인 변수

    private void Awake()
    {
        playerNameInputField.characterLimit = 10;  // 입력 글자수 제한 (10글자)
        playerNameButton.SetActive(false);
        warningText.gameObject.SetActive(false);

        // Check if there's already a saved player name
        if (!string.IsNullOrEmpty(GameManager.Instance.playerName))
        {
            // 이름이 이미 저장되어 있으면 이름을 입력하지 않고 게임 시작 가능
            isNameEntered = true;
            playerNameButton.SetActive(false);  // 처음에 이름 입력창 비활성화
        }
        else
        {
            // 이름이 없으면 이름 입력창 활성화
            EnableNameInput();
        }
    }

    public void EnableNameInput()
    {
        playerNameButton.SetActive(true);
    }

    public void SceneChange(string sceneName)
    {
        // 이름이 이미 입력된 경우 씬 이동
        if (isNameEntered)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // 이름이 입력되지 않은 경우, 이름 입력 체크
            if (string.IsNullOrWhiteSpace(playerNameInputField.text))
            {
                // 이름이 비어있을 때 경고 메시지 표시
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
                // 이름을 입력한 경우 이름을 저장하고 씬 이동 가능 상태로 변경
                GameManager.Instance.playerName = playerNameInputField.text;
                GameManager.Instance.SavePlayerData();
                isNameEntered = true;  // 이름이 입력되었다고 표시
                playerNameButton.SetActive(false);  // 입력 창 비활성화
                SceneManager.LoadScene(sceneName);  // 씬 이동
            }
        }
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
