using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Button TouchButton;
    public Text StageText;
    public Text Info;

    public void Awake()
    {
        TouchButton.onClick.AddListener(OnClickTouchButton);
    }

    public void Start()
    {
        GamePlayManager.Instance.Initialize(this);
    }

    public void ResetUI()
    {
    }

    public void GameMain()
    {
        ResetUI();
    }

    public void GameReady()
    {
        
    }

    public void GamePlay()
    {

    }

    public void GameEnd()
    {

    }

    public void GamePause()
    {

    }

    public void GameContinue()
    {

    }

    public void UpdateUI()
    {
        switch (GamePlayManager.Instance.CurrGameState)
        {
            case GamePlayManager.GAME_STATE.MAIN:
                StageText.text = string.Format("메인 화면 피 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.READY:
                StageText.text = string.Format("레디 화면 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.PLAY:
                StageText.text = string.Format("플레이 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.END:
                StageText.text = string.Format("종료 {0}", GamePlayManager.Instance.HealthPoint);
                break;
            case GamePlayManager.GAME_STATE.PAUSE:
                break;
            default:
                break;
        }
    }
   
    private void OnClickTouchButton()
    {
        GamePlayManager.Instance.ChangeGameState();
    }

    public void Update()
    {
        StringBuilder info = new StringBuilder();
        info.AppendLine(string.Format("현재 스테이지 : {0}", GamePlayManager.Instance.CurrStageData.id));
        info.AppendLine(string.Format("현재 공 속도 : {0:f2}", GamePlayManager.Instance.CurrStageData.start_speed));
        info.AppendLine(string.Format("현재 공 방향 : {0}", GamePlayManager.Instance.CurrStageData.start_rightdir ? "오른쪽" : "왼쪽"));
        info.AppendLine(string.Format("현재 체력 : {0} / {1}", GamePlayManager.Instance.HealthPoint, GamePlayManager.Instance.MaxHealthPoint));
        info.AppendLine(string.Format("현재 바퀴수 : {0}", GamePlayManager.Instance.TurnCount));
        info.AppendLine(string.Format("현재 점수 : {0}", GamePlayManager.Instance.Score));
        Info.text = info.ToString();
    }
}
