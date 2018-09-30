using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    public enum OBJECT_TYPE
    {
        NONE,
        ITEM,
        STAGE_END_LEFT,
        STAGE_END_RIGHT,
        STAGE_START,
    }
    public OBJECT_TYPE Type;
}
