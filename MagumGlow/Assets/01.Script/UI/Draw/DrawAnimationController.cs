using System.Collections;
using UnityEngine;

public class DrawAnimationController : MonoBehaviour
{
    public Animator jewelAnimator;

    public void PlayJewelAnimation(string jewelGrade)
    {
        jewelAnimator.SetTrigger(jewelGrade);
    }

    public IEnumerator WaitForAnimationEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void ShowDrawPanel(bool chk)
    {
        jewelAnimator.SetBool("OpenDraw", chk);
    }
}
