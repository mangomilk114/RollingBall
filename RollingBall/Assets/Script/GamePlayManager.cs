using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager _instance = null;
    public static GamePlayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GamePlayManager>() as GamePlayManager;
            }
            return _instance;
        }
    }

    public enum GAME_STATE
    {
        MAIN,
        READY,
        PLAY,
        END,
        PAUSE
    }

    public int StageIndex { get; private set;}
    public StageData CurrStageData { get; private set; }
    public GAME_STATE CurrGameState { get; private set; }
    public int HealthPoint { get; private set; }
    public int MaxHealthPoint { get; private set; }

    [System.NonSerialized]
    public bool GamePlayingTouch = false;
    private UIGamePlay GamePlayUI;

    public Ball PlayBall;
    public Track PlayTrack;
    public Transform CenterPos;
    public List<Item> ItemObjectList = new List<Item>();
    public CollisionObject BallStart;
    public CollisionObject BallEndCheck_Left;
    public CollisionObject BallEndCheck_Right;

    void Start()
    {
        DontDestroyOnLoad(this);
        GameMain();
    }

    public void Initialize(UIGamePlay ui)
    {
        GamePlayUI = ui;

        BallStart.SetPlace(CommonData.BALL_START_DEGREE);
        BallEndCheck_Left.SetPlace(CommonData.BALL_END_LEFT_DEGREE);
        BallEndCheck_Right.SetPlace(CommonData.BALL_END_RIGHT_DEGREE);
    }

    public void ResetGamePlay()
    {
        StageIndex = 0;
        CurrStageData = DataManager.Instance.StageDataList[StageIndex];
        HealthPoint = CommonData.DEFAULT_BALL_HEALTH_POINT;
        MaxHealthPoint = CommonData.MAX_BALL_HEALTH_POINT;
        PlayBall.Initialize(CenterPos, 1);
        PlayTrack.Initialize(CurrStageData.track_img);
        GamePlayingTouch = false;

        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            ItemObjectList[i].ResetItem();
        }
    }

    public void GameMain()
    {
        CurrGameState = GAME_STATE.MAIN;
        ResetGamePlay();
        SetStage();
    }

    public void GameReady()
    {
        CurrGameState = GAME_STATE.READY;
        ResetGamePlay();
        
    }

    public void GamePlay()
    {
        CurrGameState = GAME_STATE.PLAY;
        ResetGamePlay();
    }

    public void GameEnd()
    {
        CurrGameState = GAME_STATE.END;
    }

    public void GamePause()
    {
        CurrGameState = GAME_STATE.PAUSE;
    }

    public void GameContinue()
    {

    }

    public void ChangeGameState()
    {
        SetStage();
        return;
        switch (CurrGameState)
        {
            case GAME_STATE.MAIN:
                GameReady();
                break;
            case GAME_STATE.READY:
                GamePlay();
                break;
            case GAME_STATE.PLAY:
                GamePlayingTouch = true;
                break;
            case GAME_STATE.END:
                break;
            case GAME_STATE.PAUSE:
                break;
            default:
                break;
        }
    }


    IEnumerator CoGameUpdate()
    {
        while (true)
        {
            if (CurrGameState != GAME_STATE.PLAY)
            {
                yield return null;
                continue;
            }

            PlayBall.UpdateBall(Time.deltaTime);

            //PlayBall.BallTouchAcion(BallAction);
            //BallAction = false;

            
            //StageClearCheck();
            //StageStart();

            //if (PlayBall.GetHealthPoint() < 0)
            //    GameEnd();

            //StageText.text = string.Format("Stage - {0}", StageCount);

            yield return null;
        }
    }


    public void PlusHealthPoint(int value)
    {
        HealthPoint += value;
        if (HealthPoint >= MaxHealthPoint)
            HealthPoint = MaxHealthPoint;
    }

    public void MinusHealthPoint(int value)
    {
        HealthPoint -= value;
        if (HealthPoint <= 0)
            GameEnd();
    }

    public void SetStage()
    {
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            ItemObjectList[i].ResetItem();
        }

        var StagePresetData = DataManager.Instance.StagePresetDataDic[CurrStageData.preset];
        if (StagePresetData.itemcount > CommonData.STAGE_ALL_ITEM_COUNT)
            return;

        List<KeyValuePair<int, CommonData.ITEM_TYPE>> ItemDegreeList = new List<KeyValuePair<int, CommonData.ITEM_TYPE>>();
        var enumerator = StagePresetData.itemTypedic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            CommonData.ITEM_TYPE itemType = enumerator.Current.Key;
            int Count = enumerator.Current.Value;

            for (int index_1 = 0; index_1 < Count; index_1++)
            {
                while (true)
                {
                    int degree = Random.Range(0, 360);
                    bool addEnable = true;
                    for (int index_2 = 0; index_2 < ItemDegreeList.Count; index_2++)
                    {
                        if(degree >= ItemDegreeList[index_2].Key - CommonData.ITEM_DEGREE_GAP &&
                           degree <= ItemDegreeList[index_2].Key + CommonData.ITEM_DEGREE_GAP)
                        {
                            addEnable = false;
                            break;
                        }
                    }

                    if (degree >= CommonData.BALL_END_LEFT_DEGREE - CommonData.ITEM_DEGREE_GAP &&
                           degree <= CommonData.BALL_END_RIGHT_DEGREE + CommonData.ITEM_DEGREE_GAP)
                    {
                        addEnable = false;
                    }

                    if (addEnable)
                    {
                        ItemDegreeList.Add(new KeyValuePair<int, CommonData.ITEM_TYPE>(degree, itemType));
                        break;
                    }
                }
            } 
        }

        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            if (ItemDegreeList.Count <= i)
                break;

            ItemObjectList[i].SetPlace(ItemDegreeList[i].Key);
            ItemObjectList[i].SetData(ItemDegreeList[i].Value);
        }
    }
}