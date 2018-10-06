using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObject : MonoBehaviour
{
    [System.NonSerialized]
    public CommonData.OBJECT_TYPE Type;
    [System.NonSerialized]
    public float Degree;

    public void SetPlace(float degree)
    {
        Degree = degree;
        float dRad = -degree * 3.14f / 180f;
        float x = CommonData.TRACK_RADIUS_DISTANCE * Mathf.Sin(dRad);
        float y = CommonData.TRACK_RADIUS_DISTANCE * Mathf.Cos(dRad);
        gameObject.transform.position = new Vector3(x, y);

        gameObject.transform.rotation = Quaternion.Euler(0, 0, (float)(Degree + 180));
    }
}
