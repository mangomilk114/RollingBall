using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Ball PlayBall;
    public Transform CenterPos;

    // UI
    public Button StartBtn;
    public Button SpeedUpBtn;

    private RollingBallSystem RollingSystem = new RollingBallSystem();

    public void Awake()
    {
        SpeedUpBtn.onClick.AddListener(OnClickSpeedUp);
        StartBtn.onClick.AddListener(OnClickStart);
    }

    public void Start()
    {
        StartBtn.gameObject.SetActive(true);
        SpeedUpBtn.gameObject.SetActive(false);
    }

    private void GameStart()
    {
        RollingSystem.Initialize(CenterPos, PlayBall.gameObject.transform);
        StartCoroutine(CoGameUpdate());
    }

    IEnumerator CoGameUpdate()
    {
        while(true)
        {
            RollingSystem.UpdateRollingBall(Time.deltaTime);
            PlayBall.gameObject.transform.position = RollingSystem.GetCurrBallPos();
            yield return null;
        }
    }

    private void OnClickStart()
    {
        GameStart();
        StartBtn.gameObject.SetActive(false);
        SpeedUpBtn.gameObject.SetActive(true);
    }

    private void OnClickSpeedUp()
    {
        RollingSystem.PlusMoveSpeed(10f);
    }
}
