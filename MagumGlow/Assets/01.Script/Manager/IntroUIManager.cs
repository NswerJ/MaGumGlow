using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField PlayerName; // �÷��̾� �̸� �Է� �ʵ�


    public void SceneChange(string sceneName)
    {
        PlayerPrefs.SetString("playerName", PlayerName.text);
        PlayerPrefs.Save(); // ����� �����͸� �����ϰ� ����
        // ���Ŀ� �� ���� ������ �߰��ؾ� �� (��: SceneManager.LoadScene(sceneName);)

        SceneManager.LoadScene(sceneName);
    }
}
