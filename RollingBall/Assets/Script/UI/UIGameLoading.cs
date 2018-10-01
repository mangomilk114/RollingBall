using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameLoading : MonoBehaviour {

    void Start()
    {
        StartCoroutine(LoadingData());
    }

    IEnumerator LoadingData()
    {
        yield return DataManager.Instance.LoadingData();
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }
}
