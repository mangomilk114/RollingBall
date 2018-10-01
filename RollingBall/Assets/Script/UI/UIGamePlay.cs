using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Button TouchButton;
    public Text StageText;

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
    

    //private void StageClearCheck()
    //{
    //    if (PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_LEFT ||
    //        PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_RIGHT)
    //    {
    //        if(PlayBall.BallMoveRightDir)
    //        {
    //            if(PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_LEFT)
    //            {
    //                StageCount++;
    //                // TODO 환웅 임시
    //                if (DataManager.Instance.StageDataList.Count <= StageCount)
    //                    StageCount = 1;
    //                CurrStageData = DataManager.Instance.StageDataList[StageCount - 1];
    //                PlayTrack.Initialize(CurrStageData.TrackImg);
    //                PlayBall.ResetCollisionObject();
    //            }
    //        }
    //        else
    //        {
    //            if (PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_RIGHT)
    //            {
    //                StageCount++;
    //                // TODO 환웅 임시
    //                if (DataManager.Instance.StageDataList.Count <= StageCount)
    //                    StageCount = 1;
    //                CurrStageData = DataManager.Instance.StageDataList[StageCount - 1];
    //                PlayTrack.Initialize(CurrStageData.TrackImg);
    //                PlayBall.ResetCollisionObject();
    //            }
    //        }
            
    //    }
    //}

    //private void StageStart()
    //{
    //    if (PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_START)
    //    {
    //        PlayBall.SetStageData(CurrStageData);
    //        PlayBall.ResetCollisionObject();
    //    }
    //}

    private void OnClickTouchButton()
    {
        GamePlayManager.Instance.ChangeGameState();
    }
}
