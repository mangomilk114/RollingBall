using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : InGameObject
{
    public SpriteRenderer Img;
    private Transform CenterPos;

    private BallData Data = null;
    private StageData StageData = null;
    private float RadiusDistance = 0;
    private float MoveSpeed = 0;

    public bool BallMoveRightDir = true;


    public void Initialize(Transform centerPos, int id)
    {
        CenterPos = centerPos;

        RadiusDistance = CommonData.TRACK_RADIUS_DISTANCE;
        Data = DataManager.Instance.BallDataDic[id];
        Img.sprite = (Sprite)Resources.Load(Data.ball_img, typeof(Sprite));
    }

    public void ResetPos()
    {
        SetPlace(180f);
    }

    public void SetStageData(StageData stageData)
    {
        StageData = stageData;
        MoveSpeed = StageData.start_speed;
        BallMoveRightDir = StageData.start_rightdir;
    }

    public void UpdateBall(float time)
    {
        // BALL MOVE
        var Pos = gameObject.transform.position;

        Vector3 TargetToCenterDir = CenterPos.position - Pos;
        TargetToCenterDir.Normalize();

        var MoveDir = new Vector3();
        Vector3.OrthoNormalize(ref TargetToCenterDir, ref MoveDir);
        MoveDir *= BallMoveRightDir ? -1 : 1;
        MoveDir.Normalize();

        var TargetMovePos = Pos + (MoveDir * MoveSpeed * time);
        var TargetToCenterDis = Vector3.Distance(CenterPos.position, TargetMovePos);
        Vector3 TargetMoveRealPos = Vector3.MoveTowards(TargetMovePos, TargetToCenterDir, (TargetToCenterDis - RadiusDistance));
        Pos = TargetMoveRealPos;

        gameObject.transform.position = Pos;
    }

    public void PlusMoveSpeed(float speed)
    {
        MoveSpeed += speed;
    }

    // 등가속도 운동
    // private float ReverseSpeed = 0;
    //public void Initialize(Transform centerPos)
    //{
    //    CenterPos = centerPos;

    //    RadiusDistance = CommonData.TRACK_RADIUS_DISTANCE;

    //    // TODO 환웅 트랙의 조건에 따라 변경되는 데이터들
    //    MoveSpeed = 10f;
    //    ReverseSpeed = 0.1f;
    //    BallMoveRightDir = true;
    //}

    //public void UpdateBall(float time)
    //{
    //    // BALL MOVE
    //    var Pos = gameObject.transform.position;
    //    double PrevAngle = GetCenterToBallAngle();

    //    Vector3 TargetToCenterDir = CenterPos.position - Pos;
    //    TargetToCenterDir.Normalize();

    //    var MoveDir = new Vector3();
    //    Vector3.OrthoNormalize(ref TargetToCenterDir, ref MoveDir);
    //    MoveDir *= BallMoveRightDir ? -1 : 1;
    //    MoveDir.Normalize();

    //    var TargetMovePos = Pos + (MoveDir * MoveSpeed * time);
    //    var TargetToCenterDis = Vector3.Distance(CenterPos.position, TargetMovePos);
    //    Vector3 TargetMoveRealPos = Vector3.MoveTowards(TargetMovePos, TargetToCenterDir, (TargetToCenterDis - RadiusDistance));
    //    Pos = TargetMoveRealPos;

    //    double CurrAngle = GetCenterToBallAngle();

    //    if (BallMoveRightDir)
    //    {
    //        if (CurrAngle > 180 && PrevAngle > 180)
    //        {
    //            MoveSpeed -= ReverseSpeed;
    //            MoveSpeed -= 0.05f;
    //        }
    //        else
    //        {
    //            MoveSpeed += ReverseSpeed;
    //        }
    //    }
    //    else
    //    {
    //        if (CurrAngle > 180 && PrevAngle > 180)
    //        {
    //            MoveSpeed += ReverseSpeed;
    //        }
    //        else
    //        {
    //            MoveSpeed -= ReverseSpeed;
    //            MoveSpeed -= 0.05f;
    //        }
    //    }

    //    if (MoveSpeed <= 0)
    //    {
    //        MoveSpeed = 0;
    //        BallMoveRightDir = !BallMoveRightDir;
    //    }

    //    gameObject.transform.position = Pos;
    //}

    //private float GetCenterToBallAngle()
    //{
    //    var Pos = gameObject.transform.position;
    //    float Angle;
    //    float dX = CenterPos.position.x - Pos.x;
    //    float dY = CenterPos.position.y - Pos.y;

    //    float dRad = Mathf.Atan2(dY, dX);
    //    Angle = (float)((dRad * 180) / 3.14159265);
    //    Angle += 90;

    //    if (Angle < 0)
    //        Angle = 360 + Angle;

    //    return Angle;
    //}


}
