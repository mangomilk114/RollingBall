using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonData
{
    public enum ITEM_TYPE
    {
        NONE,
        CHEST,
        SAW,
        SPEED_DOWN,
        SPEED_UP,
    }
    public enum OBJECT_TYPE
    {
        NONE,
        ITEM,
        HERO,
        STAGE_END_LEFT,
        STAGE_END_RIGHT,
        STAGE_START,
    }

    public static float TRACK_RADIUS_DISTANCE = 4.57f;
    public static int DEFAULT_JELLY_HEALTH_POINT = 5;
    public static int MAX_JELLY_HEALTH_POINT = 5;

    public static int JELLY_START_DEGREE = 180;
    public static int JELLY_END_LEFT_DEGREE = 160;
    public static int JELLY_END_RIGHT_DEGREE = 200;
    public static int STAGE_ALL_ITEM_COUNT = 15;
    public static int ITEM_DEGREE_GAP = 15;
    public static float IN_GAMEOBJECT_CRASH_DEGREE_GAP = 15;
    public static int TOUCH_MINUS_HP = 1;
    public static int TURN_TRACK_MINUS_HP = 1;

    public static int COIN_ITEM_CRAETE_PERCENT = 10;


    public static ITEM_TYPE GetItemType(string type)
    {
        switch (type)
        {
            case "CHEST":
                return ITEM_TYPE.CHEST;
            case "SAW":
                return ITEM_TYPE.SAW;
            case "SPEED_DOWN":
                return ITEM_TYPE.SPEED_DOWN;
            case "SPEED_UP":
                return ITEM_TYPE.SPEED_UP;
        }

        return ITEM_TYPE.NONE;
    }

    static public void SetImageFile(string fileName, ref Image img, bool sizeAuto = true)
    {
        var imgSprite = (Sprite)Resources.Load(fileName, typeof(Sprite));
        if (imgSprite == null)
            return;

        if (sizeAuto)
        {
            RectTransform rt = img.GetComponent<RectTransform>();
            rt.sizeDelta = imgSprite.rect.size;
        }
        img.sprite = imgSprite;
    }
}