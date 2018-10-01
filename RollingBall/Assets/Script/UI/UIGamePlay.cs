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
