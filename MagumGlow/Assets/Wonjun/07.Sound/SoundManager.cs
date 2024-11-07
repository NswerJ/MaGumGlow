using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer _mixer;            
    public AudioSource _bgSound;        
    public AudioClip[] _bgList;           
    public Slider bgSlider;            
    public Slider sfxSlider;             
    public Slider masterSlider;           

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            SceneManager.sceneLoaded += OnSceneLoaded;  
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    private void Start()
    {
        if (bgSlider != null)
            bgSlider.value = PlayerPrefs.GetFloat("BGVolume", 0.5f);

        if (sfxSlider != null)
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        if (masterSlider != null)
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < _bgList.Length; i++)
        {
            if (scene.name == _bgList[i].name)
                BgSoundPlay(_bgList[i]);
        }

        UpdateSliders();
    }

    private void UpdateSliders()
    {
        bgSlider = GameObject.Find("BGSlider")?.GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider")?.GetComponent<Slider>();
        masterSlider = GameObject.Find("MasterSlider")?.GetComponent<Slider>();

        if (bgSlider != null)
        {
            bgSlider.onValueChanged.AddListener(BGSoundVolume);
            bgSlider.value = PlayerPrefs.GetFloat("BGVolume", 0.5f);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SFXVolume);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        }

        if (masterSlider != null)
        {
            masterSlider.onValueChanged.AddListener(MasterVolume);
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        }
    }

    public void BGSoundVolume(float val)
    {
        _mixer.SetFloat("BGSound", Mathf.Log10(val) * 20);  
        PlayerPrefs.SetFloat("BGVolume", val);               
    }

    public void SFXVolume(float val)
    {
        _mixer.SetFloat("SFXvolume", Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat("SFXVolume", val);
    }

    public void MasterVolume(float val)
    {
        _mixer.SetFloat("Master", Mathf.Log10(val) * 10);  
        PlayerPrefs.SetFloat("MasterVolume", val);
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = _mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        _bgSound.outputAudioMixerGroup = _mixer.FindMatchingGroups("BGSound")[0];
        _bgSound.clip = clip;
        _bgSound.loop = true;
        _bgSound.volume = 0.1f;
        _bgSound.Play();
    }
}
