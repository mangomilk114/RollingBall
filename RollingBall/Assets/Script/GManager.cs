using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager _instance = null;
    public static GManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GManager>() as GManager;
            }
            return _instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 50;
        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, false);

        PlayerData.Instance.Initialize();
    }
}