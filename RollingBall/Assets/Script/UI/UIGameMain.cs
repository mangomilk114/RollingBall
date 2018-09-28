using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameMain : MonoBehaviour {

    public Button StartButton;

    void Awake()
    {
        StartButton.onClick.AddListener(OnClickStart);
    }

    private void OnClickStart()
    {
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }
}
