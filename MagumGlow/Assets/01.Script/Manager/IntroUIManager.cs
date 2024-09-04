using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroUIManager : MonoBehaviour
{
    public TMP_InputField PlayerName; // 플레이어 이름 입력 필드


    public void SceneChange(string sceneName)
    {
        PlayerPrefs.SetString("playerName", PlayerName.text);
        PlayerPrefs.Save(); // 저장된 데이터를 안전하게 저장
        // 이후에 씬 변경 로직을 추가해야 함 (예: SceneManager.LoadScene(sceneName);)

        SceneManager.LoadScene(sceneName);
    }
}
