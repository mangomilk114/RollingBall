using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : CollisionObject
{
    public SpriteRenderer Img;
    private Transform CenterPos;

    private BallData Data = null;
    private StageData StageData = null;
    private float RadiusDistance = 0;
    private float MoveSpeed = 0;

    public bool BallMoveRightDir = true;

    public CollisionObject CurrCollisionObject = null;


    public void Initialize(Transform centerPos, int id)
    {
        CenterPos = centerPos;

        RadiusDistance = CommonData.TRACK_RADIUS_DISTANCE;
        Data = DataManager.Instance.BallDataDic[id];
        Img.sprite = (Sprite)Resources.Load(Data.ball_img, typeof(Sprite));

        CurrCollisionObject = null;
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

    public void BallCollisionAcion(bool touch)
    {
        if (CurrCollisionObject != null)
        {
            if ((CurrCollisionObject.Type == OBJECT_TYPE.STAGE_END_LEFT && BallMoveRightDir) ||
               (CurrCollisionObject.Type == OBJECT_TYPE.STAGE_END_RIGHT && BallMoveRightDir == false))
            {
                // 스테이지 클리어 체크
                GamePlayManager.Instance.SetStageClearCheck();
                CurrCollisionObject = null;
            }
            else if (CurrCollisionObject.Type == OBJECT_TYPE.ITEM && touch)
            {
                var itemObj = CurrCollisionObject.GetComponent<Item>();
                var removeEnable = GamePlayManager.Instance.HaveItem(itemObj);
                if (removeEnable)
                    CurrCollisionObject = null;
            }
            else if (CurrCollisionObject.Type == OBJECT_TYPE.STAGE_START)
            {
                GamePlayManager.Instance.PassStartPos();
                CurrCollisionObject = null;
            }
        }
        else
        {
            if (touch)
                GamePlayManager.Instance.MinusHealthPoint(10);
        }
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    CurrCollisionObject = collision.gameObject.GetComponent<CollisionObject>();
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    CurrCollisionObject = null;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CurrCollisionObject = collision.gameObject.GetComponent<CollisionObject>();
        Debug.Log("OnTriggerEnter2D");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CurrCollisionObject = null;
        Debug.Log("OnTriggerExit2D");
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
