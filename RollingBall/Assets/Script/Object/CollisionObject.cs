using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    public enum OBJECT_TYPE
    {
        NONE,
        ITEM,
        BALL,
        STAGE_END_LEFT,
        STAGE_END_RIGHT,
        STAGE_START,
    }
    public OBJECT_TYPE Type;

    public void SetPlace(float degree)
    {
        float dRad = -degree * 3.14f / 180f;
        float x = CommonData.TRACK_RADIUS_DISTANCE * Mathf.Sin(dRad);
        float y = CommonData.TRACK_RADIUS_DISTANCE * Mathf.Cos(dRad);
        gameObject.transform.position = new Vector3(x, y);
    }
}
