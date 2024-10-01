using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInputField;  // �÷��̾� �̸� �Է� �ʵ�
    public GameObject playerNameButton;  // �÷��̾� �̸� �Է� �ʵ� �θ�
    public TMP_Text warningText;  // ��� �޽����� ǥ���� �ؽ�Ʈ
    private Coroutine fadeCoroutine;  // ���� ���� ���� ���̵� �ƿ� �ڷ�ƾ�� �����ϱ� ���� ����
    private bool isNameEntered = false; // �̸��� �ԷµǾ����� ���� Ȯ�� ����

    private void Awake()
    {
        playerNameInputField.characterLimit = 10;  // �Է� ���ڼ� ���� (10����)
        playerNameButton.SetActive(false);
        warningText.gameObject.SetActive(false);

        // Check if there's already a saved player name
        if (!string.IsNullOrEmpty(GameManager.Instance.playerName))
        {
            // �̸��� �̹� ����Ǿ� ������ �̸��� �Է����� �ʰ� ���� ���� ����
            isNameEntered = true;
            playerNameButton.SetActive(false);  // ó���� �̸� �Է�â ��Ȱ��ȭ
        }
        else
        {
            // �̸��� ������ �̸� �Է�â Ȱ��ȭ
            EnableNameInput();
        }
    }

    public void EnableNameInput()
    {
        playerNameButton.SetActive(true);
    }

    public void SceneChange(string sceneName)
    {
        // �̸��� �̹� �Էµ� ��� �� �̵�
        if (isNameEntered)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // �̸��� �Էµ��� ���� ���, �̸� �Է� üũ
            if (string.IsNullOrWhiteSpace(playerNameInputField.text))
            {
                // �̸��� ������� �� ��� �޽��� ǥ��
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
                // �̸��� �Է��� ��� �̸��� �����ϰ� �� �̵� ���� ���·� ����
                GameManager.Instance.playerName = playerNameInputField.text;
                GameManager.Instance.SavePlayerData();
                isNameEntered = true;  // �̸��� �ԷµǾ��ٰ� ǥ��
                playerNameButton.SetActive(false);  // �Է� â ��Ȱ��ȭ
                SceneManager.LoadScene(sceneName);  // �� �̵�
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
