using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBallSystem
{
    private Transform CenterPos;
    private Transform BallPos;

    private float RadiusDistance = 0;
    private float MoveSpeed = 0;
    private float ReverseSpeed = 0;
    private bool BallMoveRightDir = true;

    public void Initialize(Transform centerPos, Transform ballPos)
    {
        CenterPos = centerPos;
        BallPos = ballPos;

        RadiusDistance = 5f;
        MoveSpeed = 5f;
        ReverseSpeed = 0.1f;
        BallMoveRightDir = true;
    }

    public void UpdateRollingBall(float time)
    {
        double PrevAngle = GetCenterToBallAngle();

        Vector3 TargetToCenterDir = CenterPos.position - BallPos.position;
        TargetToCenterDir.Normalize();

        var MoveDir = new Vector3();
        Vector3.OrthoNormalize(ref TargetToCenterDir, ref MoveDir);
        MoveDir *= BallMoveRightDir ? -1 : 1;
        MoveDir.Normalize();

        var TargetMovePos = BallPos.position + (MoveDir * MoveSpeed * time);
        var TargetToCenterDis = Vector3.Distance(CenterPos.position, TargetMovePos);
        Vector3 TargetMoveRealPos = Vector3.MoveTowards(TargetMovePos, TargetToCenterDir, (TargetToCenterDis - RadiusDistance));
        BallPos.position = TargetMoveRealPos;

        double CurrAngle = GetCenterToBallAngle();

        if (BallMoveRightDir)
        {
            if (CurrAngle > 180 && PrevAngle > 180)
            {
                MoveSpeed -= ReverseSpeed;
                MoveSpeed -= 0.05f;
            }
            else
            {
                MoveSpeed += ReverseSpeed;
            }
        }
        else
        {
            if (CurrAngle > 180 && PrevAngle > 180)
            {
                MoveSpeed += ReverseSpeed;
            }
            else
            {
                MoveSpeed -= ReverseSpeed;
                MoveSpeed -= 0.05f;
            }
        }

        if (MoveSpeed <= 0)
        {
            MoveSpeed = 0;
            BallMoveRightDir = !BallMoveRightDir;
        }
    }

    public void PlusMoveSpeed(float speed)
    {
        MoveSpeed += speed;
    }

    public Vector3 GetCurrBallPos()
    {
        return BallPos.position;
    }

    private float GetCenterToBallAngle()
    {
        float Angle;
        float dX = CenterPos.position.x - BallPos.position.x;
        float dY = CenterPos.position.y - BallPos.position.y;

        float dRad = Mathf.Atan2(dY, dX);
        Angle = (float)((dRad * 180) / 3.14159265);
        Angle += 90;

        if (Angle < 0)
            Angle = 360 + Angle;

        return Angle;
    }
}
