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
    [System.NonSerialized]
    public int HaveChestCount = 0;

    private UIGamePlay GamePlayUI;

    public JellyHeroChar PlayJellyHero;
    public SpriteRenderer BackgroundImg;
    public Transform CenterPos;
    public List<Item> ItemObjectList = new List<Item>();
    public InGameObject JellyHeroStart;
    public InGameObject JellyHeroEndCheck_Left;
    public InGameObject JellyHeroEndCheck_Right;
    public CommonData.OBJECT_TYPE JellyHeroCrashType = CommonData.OBJECT_TYPE.NONE;
    public List<JellyHPChar> JellyHPCharObjectList = new List<JellyHPChar>();
    public List<GameObject> ChestObjectList = new List<GameObject>();

    void Start()
    {
        DontDestroyOnLoad(this);
        GameMain();
    }

    public void Initialize(UIGamePlay ui)
    {
        GamePlayUI = ui;

        JellyHeroStart.SetPlace(CommonData.JELLY_START_DEGREE);
        JellyHeroEndCheck_Left.SetPlace(CommonData.JELLY_END_LEFT_DEGREE);
        JellyHeroEndCheck_Right.SetPlace(CommonData.JELLY_END_RIGHT_DEGREE);
    }

    public void ResetGamePlay()
    {
        var bgData = DataManager.Instance.BackgroundDataDic[PlayerData.Instance.BackgroundId];
        BackgroundImg.sprite = (Sprite)Resources.Load(bgData.img, typeof(Sprite));

        StageIndex = PlayerData.Instance.StageIndex;
        CurrStageData = DataManager.Instance.StageDataList[StageIndex];
        HealthPoint = CommonData.DEFAULT_JELLY_HEALTH_POINT;
        MaxHealthPoint = CommonData.MAX_JELLY_HEALTH_POINT;
        PlayJellyHero.Initialize(CenterPos, PlayerData.Instance.JellyCharId);
        
        GamePlayingTouch = false;
        IsStageClear = false;
        ResetStage();
    }

    public void GameMain()
    {
        CurrGameState = GAME_STATE.MAIN;
        ResetGamePlay();
        PlayJellyHero.SetStageData(CurrStageData);
        StopAllCoroutines();
        StartCoroutine(CoGameUpdate());
        GamePlayUI.GameMain();
    }

    public void GameReady()
    {
        JellyHeroCrashType = CommonData.OBJECT_TYPE.STAGE_START;
        CurrGameState = GAME_STATE.READY;
        ResetGamePlay();
        SetStage();
        PlayJellyHero.ResetPos();
        GamePlayUI.GameReady();
    }

    public void GamePlay()
    {
        CurrGameState = GAME_STATE.PLAY;
        GamePlayUI.GamePlay();
    }

    public void GameEnd()
    {
        CurrGameState = GAME_STATE.END;
        ResetStage();
        PlayJellyHero.ResetPos();
        GamePlayUI.GameEnd();
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
                GameMain();
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
            if (CurrGameState == GAME_STATE.READY ||
                CurrGameState == GAME_STATE.END ||
                CurrGameState == GAME_STATE.PAUSE)
            {
                yield return null;
                continue;
            }
            if (CurrGameState == GAME_STATE.PLAY)
                JellyHeroCrashAcion(GamePlayingTouch);
            GamePlayingTouch = false;
            PlayJellyHero.UpdateJellyHero(Time.deltaTime);
            yield return null;
        }
    }


    public void PlusHealthPoint(int value)
    {
        HealthPoint += value;
        if (HealthPoint >= MaxHealthPoint)
            HealthPoint = MaxHealthPoint;
        ChangeJellyHPCharCount();
    }

    public void MinusHealthPoint(int value)
    {
        HealthPoint -= value;
        ChangeJellyHPCharCount();
        if (HealthPoint <= 0)
            GameEnd();
    }

    public void ResetStage()
    {
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            ItemObjectList[i].ResetItem();
        }

        for (int i = 0; i < ChestObjectList.Count; i++)
        {
            ChestObjectList[i].gameObject.SetActive(false);
        }

        HaveChestCount = 0;
        ChangeChestCount();
    }

    public void SetStage()
    {
        ResetStage();

        var StagePresetData = DataManager.Instance.StagePresetDataDic[CurrStageData.preset];

        List<KeyValuePair<int, CommonData.ITEM_TYPE>> ItemDegreeList = new List<KeyValuePair<int, CommonData.ITEM_TYPE>>();
        var itemList = StagePresetData.GetPresetItemList();
        for (int listIndex = 0; listIndex < itemList.Count; listIndex++)
        {
            var ItemType = itemList[listIndex].Key;
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

                    if (degree >= CommonData.JELLY_END_LEFT_DEGREE - CommonData.ITEM_DEGREE_GAP &&
                           degree <= CommonData.JELLY_END_RIGHT_DEGREE + CommonData.ITEM_DEGREE_GAP)
                    {
                        addEnable = false;
                    }

                    if (addEnable)
                    {
                        ItemDegreeList.Add(new KeyValuePair<int, CommonData.ITEM_TYPE>(degree, ItemType));
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
            case CommonData.ITEM_TYPE.CHEST:
                HaveChestCount++;
                ChangeChestCount();
                break;
            case CommonData.ITEM_TYPE.SAW:
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
            case CommonData.ITEM_TYPE.CHEST:
                return;
            case CommonData.ITEM_TYPE.SAW:
                {
                    float gap = GetTargetToObjectAngleGap(GetCenterToJellyHeroAngle(), item);
                    if (gap < 1f)
                    {
                        MinusHealthPoint(item.Data.value);
                        item.ResetItem();
                    }
                    break;
                }
                
            default:
                break;
        }
        
    }
    public void SetStageClearCheck()
    {
        bool stageClear = true;
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            if (ItemObjectList[i].ItemType == CommonData.ITEM_TYPE.CHEST)
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
            GamePlayUI.ChangeStageCount();
            SetStage();
        }
    }

    public void PassStartPos()
    {
        if (IsStageClear)
        {
            PlusHealthPoint(CommonData.DEFAULT_JELLY_HEALTH_POINT);
            PlayJellyHero.SetStageData(CurrStageData);
            IsStageClear = false;
            TurnCount = 0;
        }
        else
        {
            TurnCount++;
            MinusHealthPoint(CommonData.TURN_TRACK_MINUS_HP);
            PlayJellyHero.SetStageData(CurrStageData);
        }
    }


    public void JellyHeroCrashAcion(bool touch)
    {
        bool minusHealthPointEnable = true;
        var crashObject = GetJellyHeroToObjectCrashObject();
        if (crashObject == null)
        {
            JellyHeroCrashType = CommonData.OBJECT_TYPE.NONE;
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
                    if (JellyHeroCrashType == crashObject.Type)
                        return;
                    if (PlayJellyHero.JellyMoveRightDir)
                        SetStageClearCheck();
                    break;
                case CommonData.OBJECT_TYPE.STAGE_END_RIGHT:
                    if (JellyHeroCrashType == crashObject.Type)
                        return;
                    if (PlayJellyHero.JellyMoveRightDir == false)
                        SetStageClearCheck();
                    break;
                case CommonData.OBJECT_TYPE.STAGE_START:
                    if (JellyHeroCrashType == crashObject.Type)
                        return;
                    PassStartPos();
                    break;
                default:
                    break;
            }

            JellyHeroCrashType = crashObject.Type;
        }

        if (touch && minusHealthPointEnable)
        {
            MinusHealthPoint(CommonData.TOUCH_MINUS_HP);
        }
    }

    private float GetCenterToJellyHeroAngle()
    {
        var Pos = PlayJellyHero.gameObject.transform.position;
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

    private InGameObject GetJellyHeroToObjectCrashObject()
    {
        InGameObject crashObject = null;
        var jellyHeroAngle = GetCenterToJellyHeroAngle();
        float minGap = float.MaxValue;
        int minGapIndex = -1;
        for (int i = 0; i < ItemObjectList.Count; i++)
        {
            if (ItemObjectList[i].UniqueIndex >= 0)
            {
                float gap = GetTargetToObjectAngleGap(jellyHeroAngle, ItemObjectList[i]);

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
            float startCheckGap = GetTargetToObjectAngleGap(jellyHeroAngle, JellyHeroStart);
            if (CommonData.IN_GAMEOBJECT_CRASH_DEGREE_GAP >= startCheckGap)
                crashObject = JellyHeroStart;

            if (PlayJellyHero.JellyMoveRightDir)
            {
                float LeftCheckGap = GetTargetToObjectAngleGap(jellyHeroAngle, JellyHeroEndCheck_Left);
                if (CommonData.IN_GAMEOBJECT_CRASH_DEGREE_GAP >= LeftCheckGap)
                    crashObject = JellyHeroEndCheck_Left;
            }
            else
            {
                float RightCheckGap = GetTargetToObjectAngleGap(jellyHeroAngle, JellyHeroEndCheck_Right);
                if (CommonData.IN_GAMEOBJECT_CRASH_DEGREE_GAP >= RightCheckGap)
                    crashObject = JellyHeroEndCheck_Right;
            }
        }
        

        return crashObject;
    }


    public void ChangeJellyHPCharCount()
    {
        for (int i = 0; i < JellyHPCharObjectList.Count; i++)
        {
            JellyHPCharObjectList[i].gameObject.SetActive(HealthPoint > i);
        }
    }

    public void ChangeChestCount()
    {
        for (int i = 0; i < ChestObjectList.Count; i++)
        {
            ChestObjectList[i].gameObject.SetActive(HaveChestCount > i);
        }
    }
}