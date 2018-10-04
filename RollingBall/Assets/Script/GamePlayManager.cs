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
        NONE,
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
    [System.NonSerialized]
    public bool IsStageClear = false;
    [System.NonSerialized]
    public int TurnCount = 1;

    private UIGamePlay GamePlayUI;

    public Ball PlayBall;
    public Track PlayTrack;
    public Transform CenterPos;
    public List<Item> ItemObjectList = new List<Item>();
    public InGameObject BallStart;
    public InGameObject BallEndCheck_Left;
    public InGameObject BallEndCheck_Right;
    public CommonData.OBJECT_TYPE BallCrashType = CommonData.OBJECT_TYPE.NONE;

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
        StageIndex = PlayerData.Instance.StageIndex;
        CurrStageData = DataManager.Instance.StageDataList[StageIndex];
        HealthPoint = CommonData.DEFAULT_BALL_HEALTH_POINT;
        MaxHealthPoint = CommonData.MAX_BALL_HEALTH_POINT;
        PlayBall.Initialize(CenterPos, 1);
        PlayTrack.Initialize(CurrStageData.track_img);
        GamePlayingTouch = false;
        IsStageClear = false;
        ResetStage();
    }

    public void GameMain()
    {
        CurrGameState = GAME_STATE.MAIN;
        ResetGamePlay();
        PlayBall.SetStageData(CurrStageData);
        StartCoroutine(CoGameUpdate());
    }

    public void GameReady()
    {
        CurrGameState = GAME_STATE.READY;
        ResetGamePlay();
        SetStage();
        PlayBall.ResetPos();
    }

    public void GamePlay()
    {
        CurrGameState = GAME_STATE.PLAY;
    }

    public void GameEnd()
    {
        CurrGameState = GAME_STATE.END;
        ResetStage();
        PlayBall.ResetPos();
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
                GameReady();
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
            GamePlayUI.UpdateUI();

            if (CurrGameState == GAME_STATE.READY ||
                CurrGameState == GAME_STATE.END ||
                CurrGameState == GAME_STATE.PAUSE)
            {
                yield return null;
                continue;
            }
            if (CurrGameState == GAME_STATE.PLAY)
                BallCrashAcion(GamePlayingTouch);
            GamePlayingTouch = false;
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

    public void ResetStage()
    {
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            ItemObjectList[i].ResetItem();
        }
    }

    public void SetStage()
    {
        ResetStage();

        var StagePresetData = DataManager.Instance.StagePresetDataDic[CurrStageData.preset];

        List<KeyValuePair<int, string>> ItemDegreeList = new List<KeyValuePair<int, string>>();
        var itemList = StagePresetData.GetPresetItemList();
        for (int listIndex = 0; listIndex < itemList.Count; listIndex++)
        {
            string ItemName = itemList[listIndex].Key;
            int Count = itemList[listIndex].Value;

            for (int index_1 = 0; index_1 < Count; index_1++)
            {
                while (true)
                {
                    int degree = Random.Range(0, 360);
                    bool addEnable = true;
                    for (int index_2 = 0; index_2 < ItemDegreeList.Count; index_2++)
                    {
                        float gap = GetTargetToObjectAngleGap(ItemDegreeList[index_2].Key, degree);
                        if (gap < CommonData.ITEM_DEGREE_GAP)
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
                        ItemDegreeList.Add(new KeyValuePair<int, string>(degree, ItemName));
                        break;
                    }

                    if (ItemDegreeList.Count >= CommonData.STAGE_ALL_ITEM_COUNT)
                        break;
                }
                if (ItemDegreeList.Count >= CommonData.STAGE_ALL_ITEM_COUNT)
                    break;
            }
            if (ItemDegreeList.Count >= CommonData.STAGE_ALL_ITEM_COUNT)
                break;
        }

        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            if (ItemDegreeList.Count <= i)
                break;

            ItemObjectList[i].SetPlace(ItemDegreeList[i].Key);
            ItemObjectList[i].SetData(ItemDegreeList[i].Value, i);
        }
    }

    private bool RemoveItem(Item item)
    {
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            if(ItemObjectList[i].UniqueIndex == item.UniqueIndex)
            {
                ItemObjectList[i].ResetItem();
                return true;
            }
        }
        return false;
    }

    public void HaveItem(Item item)
    {
        switch (item.ItemType)
        {
            case CommonData.ITEM_TYPE.POTION:
                PlusHealthPoint(item.Data.value);
                break;
            case CommonData.ITEM_TYPE.COIN:
                PlayerData.Instance.PlusScore(item.Data.value);
                break;
            case CommonData.ITEM_TYPE.STAR:
                break;
            case CommonData.ITEM_TYPE.SPEED_UP:
                break;
            case CommonData.ITEM_TYPE.SPEED_DOWN:
                break;
            case CommonData.ITEM_TYPE.BOMB:
                break;
            default:
                break;
        }
        item.ResetItem();
    }
    public void PassItem(Item item)
    {
        switch (item.ItemType)
        {
            case CommonData.ITEM_TYPE.STAR:
            case CommonData.ITEM_TYPE.COIN:
            case CommonData.ITEM_TYPE.POTION:
                return;
            case CommonData.ITEM_TYPE.SPEED_UP:
                PlayBall.PlusMoveSpeed(item.Data.value);
                break;
            case CommonData.ITEM_TYPE.SPEED_DOWN:
                PlayBall.PlusMoveSpeed(-item.Data.value);
                break;
            case CommonData.ITEM_TYPE.BOMB:
                MinusHealthPoint(item.Data.value);
                break;
            default:
                break;
        }
        item.ResetItem();
    }
    public void SetStageClearCheck()
    {
        bool stageClear = true;
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            if (ItemObjectList[i].ItemType == CommonData.ITEM_TYPE.STAR)
            {
                stageClear = false;
                break;
            }
        }

        IsStageClear = stageClear;

        if (stageClear)
        {
            StageIndex++;

            // TODO 환웅 임시
            if (DataManager.Instance.StageDataList.Count <= StageIndex)
                StageIndex = 0;

            PlayerData.Instance.SetStageIndex(StageIndex);
            CurrStageData = DataManager.Instance.StageDataList[StageIndex];
            SetStage();
        }
    }

    public void PassStartPos()
    {
        if (IsStageClear)
        {
            HealthPoint = CommonData.DEFAULT_BALL_HEALTH_POINT;
            PlayBall.SetStageData(CurrStageData);
            IsStageClear = false;
            TurnCount = 0;
        }
        else
        {
            TurnCount++;
            MinusHealthPoint(CommonData.TURN_TRACK_MINUS_HP);
            PlayBall.SetStageData(CurrStageData);
        }
            
    }


    public void BallCrashAcion(bool touch)
    {
        bool minusHealthPointEnable = true;
        var crashObject = GetBallToObjectCrashObject();
        if (crashObject == null)
        {
            BallCrashType = CommonData.OBJECT_TYPE.NONE;
        }
        else
        {
            switch (crashObject.Type)
            {
                case CommonData.OBJECT_TYPE.ITEM:
                    minusHealthPointEnable = false;
                    var itemObj = crashObject.GetComponent<Item>();
                    if (touch)
                        HaveItem(itemObj);
                    else
                        PassItem(itemObj);
                    break;
                case CommonData.OBJECT_TYPE.STAGE_END_LEFT:
                    if (BallCrashType == CommonData.OBJECT_TYPE.STAGE_END_LEFT)
                        return;
                    if (PlayBall.BallMoveRightDir)
                        SetStageClearCheck();
                    break;
                case CommonData.OBJECT_TYPE.STAGE_END_RIGHT:
                    if (BallCrashType == CommonData.OBJECT_TYPE.STAGE_END_RIGHT)
                        return;
                    if (PlayBall.BallMoveRightDir == false)
                        SetStageClearCheck();
                    break;
                case CommonData.OBJECT_TYPE.STAGE_START:
                    if (BallCrashType == CommonData.OBJECT_TYPE.STAGE_START)
                        return;
                    PassStartPos();
                    break;
                default:
                    break;
            }

            BallCrashType = crashObject.Type;
        }
        

        if (touch && minusHealthPointEnable)
            MinusHealthPoint(CommonData.TOUCH_MINUS_HP);
    }

    private float GetCenterToBallAngle()
    {
        var Pos = PlayBall.gameObject.transform.position;
        float Angle;
        float dX = CenterPos.position.x - Pos.x;
        float dY = CenterPos.position.y - Pos.y;

        float dRad = Mathf.Atan2(dY, dX);
        Angle = (float)((dRad * 180) / 3.14159265);
        Angle += 90;

        if (Angle < 0)
            Angle = 360 + Angle;

        return Angle;
    }

    private float GetTargetToObjectAngleGap(float target, InGameObject obj)
    {
        return GetTargetToObjectAngleGap(target, obj.Degree);
    }

    private float GetTargetToObjectAngleGap(float target, float degree)
    {
        float gap = 0f;
        if (target > 270f && target < 360f &&
               degree > 0f && degree < 90f)
        {
            gap = (degree + 360f) - target;
        }
        else if (target > 0f && target < 90f &&
           degree > 270f && degree < 360f)
        {
            gap = (target + 360f) - degree;
        }
        else if (target > degree)
            gap = target - degree;
        else
            gap = degree - target;

        return Mathf.Abs(gap);
    }

    private InGameObject GetBallToObjectCrashObject()
    {
        InGameObject crashObject = null;
        var ballAngle = GetCenterToBallAngle();
        float minGap = float.MaxValue;
        int minGapIndex = -1;
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            if (ItemObjectList[i].UniqueIndex >= 0)
            {
                float gap = GetTargetToObjectAngleGap(ballAngle, ItemObjectList[i]);

                if (minGap > gap)
                {
                    minGap = gap;
                    minGapIndex = i;
                }
            }
        }

        

        if (CommonData.IN_GAMEOBJECT_CRASH_DEGREE_GAP >= minGap && minGapIndex >= 0)
        {
            crashObject = ItemObjectList[minGapIndex];
        }

        if(crashObject == null)
        {
            float startCheckGap = GetTargetToObjectAngleGap(ballAngle, BallStart);
            if (CommonData.IN_GAMEOBJECT_CRASH_DEGREE_GAP >= startCheckGap)
                crashObject = BallStart;

            if (PlayBall.BallMoveRightDir)
            {
                float LeftCheckGap = GetTargetToObjectAngleGap(ballAngle, BallEndCheck_Left);
                if (CommonData.IN_GAMEOBJECT_CRASH_DEGREE_GAP >= LeftCheckGap)
                    crashObject = BallEndCheck_Left;
            }
            else
            {
                float RightCheckGap = GetTargetToObjectAngleGap(ballAngle, BallEndCheck_Right);
                if (CommonData.IN_GAMEOBJECT_CRASH_DEGREE_GAP >= RightCheckGap)
                    crashObject = BallEndCheck_Right;
            }
        }
        

        return crashObject;
    }
}