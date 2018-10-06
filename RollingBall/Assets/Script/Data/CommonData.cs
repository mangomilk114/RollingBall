using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData
{
    public enum ITEM_TYPE
    {
        NONE,
        CHEST,
        SAW,
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
    public static int DEFAULT_JELLY_HEALTH_POINT = 100;
    public static int MAX_JELLY_HEALTH_POINT = 100;

    public static int JELLY_START_DEGREE = 180;
    public static int JELLY_END_LEFT_DEGREE = 160;
    public static int JELLY_END_RIGHT_DEGREE = 200;
    public static int STAGE_ALL_ITEM_COUNT = 15;
    public static int ITEM_DEGREE_GAP = 15;
    public static float IN_GAMEOBJECT_CRASH_DEGREE_GAP = 10;
    public static int TOUCH_MINUS_HP = 10;
    public static int TURN_TRACK_MINUS_HP = 5;

    public static int COIN_ITEM_CRAETE_PERCENT = 10;


    public static ITEM_TYPE GetItemType(string type)
    {
        switch (type)
        {
            case "CHEST":
                return ITEM_TYPE.CHEST;
            case "SAW":
                return ITEM_TYPE.SAW;
        }

        return ITEM_TYPE.NONE;
    }
}