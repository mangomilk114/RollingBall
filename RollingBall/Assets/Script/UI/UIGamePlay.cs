using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public Ball PlayBall;
    public Track PlayTrack;
    public Transform CenterPos;

    // UI
    public Button TouchButton;
    public Text StageText;

    private enum GAME_STATE
    {
        MAIN,
        READY,
        PLAY,
        END,
        PAUSE
    }

    private int StageCount = 1;
    private StageData CurrStageData;

    private GAME_STATE GameState = GAME_STATE.MAIN;
    private bool BallAction = false;

    public void Awake()
    {
        GameState = GAME_STATE.MAIN;
        TouchButton.onClick.AddListener(OnClickTouchButton);
    }

    public void Start()
    {
        GameMain();
    }

    private void PlayReset()
    {
        StageCount = 1;
        CurrStageData = DataManager.Instance.StageDataList[StageCount - 1];
        GameState = GAME_STATE.MAIN;
    }

    private void GameMain()
    {
        PlayReset();
    }

    private void GameReady()
    {
        PlayReset();
        GameState = GAME_STATE.READY;
        PlayBall.Initialize(CenterPos, 1);
        PlayTrack.Initialize(CurrStageData.TrackImg);
    }

    private void GameStart()
    {
        GameState = GAME_STATE.PLAY;
        PlayBall.SetStageData(CurrStageData);
        StartCoroutine(CoGameUpdate());
    }

    private void GamePlayAction()
    {
        BallAction = true;
    }

    private void GameEnd()
    {
        PlayReset();
        GameState = GAME_STATE.END;
        // TODO 환웅 게임종료후 어떻게 할까??
    }

    private void GamePause()
    {
        GameState = GAME_STATE.PAUSE;
    }

    IEnumerator CoGameUpdate()
    {
        while(true)
        {
            if (GameState != GAME_STATE.PLAY)
            {
                yield return null;
                continue;
            }

            PlayBall.BallTouchAcion(BallAction);
            BallAction = false;

            PlayBall.UpdateBall(Time.deltaTime);
            StageClearCheck();
            StageStart();

            if (PlayBall.GetHealthPoint() < 0)
                GameEnd();

            StageText.text = string.Format("Stage - {0}", StageCount);

            yield return null;
        }
    }

    private void StageClearCheck()
    {
        if (PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_LEFT ||
            PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_RIGHT)
        {
            if(PlayBall.BallMoveRightDir)
            {
                if(PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_LEFT)
                {
                    StageCount++;
                    // TODO 환웅 임시
                    if (DataManager.Instance.StageDataList.Count <= StageCount)
                        StageCount = 1;
                    CurrStageData = DataManager.Instance.StageDataList[StageCount - 1];
                    PlayTrack.Initialize(CurrStageData.TrackImg);
                    PlayBall.ResetCollisionObject();
                }
            }
            else
            {
                if (PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_END_RIGHT)
                {
                    StageCount++;
                    // TODO 환웅 임시
                    if (DataManager.Instance.StageDataList.Count <= StageCount)
                        StageCount = 1;
                    CurrStageData = DataManager.Instance.StageDataList[StageCount - 1];
                    PlayTrack.Initialize(CurrStageData.TrackImg);
                    PlayBall.ResetCollisionObject();
                }
            }
            
        }
    }

    private void StageStart()
    {
        if (PlayBall.GetTriggerObjectType() == CollisionObject.OBJECT_TYPE.STAGE_START)
        {
            PlayBall.SetStageData(CurrStageData);
            PlayBall.ResetCollisionObject();
        }
    }

    private void OnClickTouchButton()
    {
        switch (GameState)
        {
            case GAME_STATE.MAIN:
                GameReady();
                break;
            case GAME_STATE.READY:
                GameStart();
                break;
            case GAME_STATE.PLAY:
                GamePlayAction();
                break;
            default:
                break;
        }
    }
}
