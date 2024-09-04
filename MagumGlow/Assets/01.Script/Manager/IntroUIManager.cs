using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField playerName;  // �÷��̾� �̸� �Է� �ʵ�
    public TMP_Text warningText;  // ��� �޽����� ǥ���� �ؽ�Ʈ
    
    private Coroutine fadeCoroutine;  // ���� ���� ���� ���̵� �ƿ� �ڷ�ƾ�� �����ϱ� ���� ����
    

    private void Awake()
    {
        playerName.gameObject.SetActive(false);
        warningText.gameObject.SetActive(false);  
    }

    public void EnableNameInput()
    {
        playerName.gameObject.SetActive(true);
    }

    public void SceneChange(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(playerName.text))
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
            PlayerPrefs.SetString("playerName", playerName.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene(sceneName);
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
