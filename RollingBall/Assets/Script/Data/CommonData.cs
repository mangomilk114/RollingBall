using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData
{
    public enum ITEM_TYPE
    {
        NONE,
        STAR,
        SPEED_UP,
        SPEED_DOWN,
        BOMB,
        COIN,
        POTION,
    }
    public enum OBJECT_TYPE
    {
        NONE,
        ITEM,
        BALL,
        STAGE_END_LEFT,
        STAGE_END_RIGHT,
        STAGE_START,
    }

    public static float TRACK_RADIUS_DISTANCE = 4.57f;
    public static int DEFAULT_BALL_HEALTH_POINT = 100;
    public static int MAX_BALL_HEALTH_POINT = 100;

    public static int BALL_START_DEGREE = 180;
    public static int BALL_END_LEFT_DEGREE = 160;
    public static int BALL_END_RIGHT_DEGREE = 200;
    public static int STAGE_ALL_ITEM_COUNT = 15;
    public static int ITEM_DEGREE_GAP = 15;
    public static float IN_GAMEOBJECT_CRASH_DEGREE_GAP = 10;


}