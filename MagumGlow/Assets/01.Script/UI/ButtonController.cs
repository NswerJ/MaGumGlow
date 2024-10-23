using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public void SetButtonInteractable(Button button, bool isInteractable)
    {
        button.interactable = isInteractable;
    }
}
