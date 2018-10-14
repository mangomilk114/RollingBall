using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSoundManager : MonoBehaviour
{
    public static FxSoundManager _instance = null;
    public static FxSoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FxSoundManager>() as FxSoundManager;
            }
            return _instance;
        }
    }

    public enum SOUND_TYPE
    {
        CHEST,
        DEMAGE,
    }

    private int soundFX;
    public AudioClip[] mFxSound = new AudioClip[14];
    private AudioSource mFxAudio;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        mFxAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayFXSound(SOUND_TYPE type)
    {
        if (PlayerData.Instance.SoundEnable == true)
        {
            mFxAudio.clip = mFxSound[(int)type];
            mFxAudio.Play();
        }
    }
}
