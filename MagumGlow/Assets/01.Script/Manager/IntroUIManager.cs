using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInputField;  // �÷��̾� �̸� �Է� �ʵ�
    public GameObject playerNameButton;  // �÷��̾� �̸� �Է� �ʵ� �θ�
    public GameObject startButton;  // �÷��̾� �̸� �Է� �ʵ� �θ�
    public GameObject orginstartButton;  // �÷��̾� �̸� �Է� �ʵ� �θ�
    public TMP_Text warningText;  // ��� �޽����� ǥ���� �ؽ�Ʈ
    private Coroutine fadeCoroutine;  // ���� ���� ���� ���̵� �ƿ� �ڷ�ƾ�� �����ϱ� ���� ����

    private void Awake()
    {
        playerNameInputField.characterLimit = 10;  // �Է� ���ڼ� ���� (10����)
        startButton.SetActive(false);
        orginstartButton.SetActive(false);
        warningText.gameObject.SetActive(false);

        // �̹� ����� �÷��̾� �̸��� �ִ��� Ȯ��
        if (!string.IsNullOrEmpty(GameManager.Instance.playerData.playerName))
        {
            EnableSceneChangeButton();
            Debug.Log("�̸��� ����");
        }
        else
        {
            EnableNameInput();
            Debug.Log("�̸� �����");
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
            warningText.text = "�̸��� �Է��ϼ���!";
            warningText.gameObject.SetActive(true);

            fadeCoroutine = StartCoroutine(FadeOutWarningText());
        }
        else
        {
            // �÷��̾� �̸��� �����ϰ� ���� ����
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
