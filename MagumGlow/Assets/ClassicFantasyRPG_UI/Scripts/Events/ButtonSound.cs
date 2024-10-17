using UnityEngine;

namespace Events
{
    public class ButtonSound : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip clickSound;

        private void Awake()
        {
            audioSource =GameObject.Find("SoundManager").GetComponent<AudioSource>();   
        }

        public void PlayButtonClick()
        {
            SoundManager.Instance.SFXPlay("ButtonClick",clickSound);
        }
    }
}