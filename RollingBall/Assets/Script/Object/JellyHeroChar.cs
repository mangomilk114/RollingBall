using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyHeroChar : InGameObject
{
    public Animator Anim = null;
    public SpriteRenderer TalkImg;
    private Transform CenterPos;

    private JellyData Data = null;
    private StageData StageData = null;

    public float MoveSpeed { get; private set; }
    public float StartMoveSpeed { get; private set; }
    public float MoveSpeedOffsetPsercent { get; private set; }
    public bool JellyMoveRightDir { get; private set; }

    public void Initialize(Transform centerPos, int id)
    {
        CenterPos = centerPos;

        Data = DataManager.Instance.JellyHeroDataDic[id];
        Anim.SetTrigger("Run");
    }

    public void ResetPos()
    {
        SetPlace(180f);
    }

    public void SetStageData(StageData stageData)
    {
        StageData = stageData;
        StartMoveSpeed = StageData.start_speed;
        MoveSpeedOffsetPsercent = 0f;
        MoveSpeed = StageData.start_speed;
        JellyMoveRightDir = StageData.start_rightdir;
        gameObject.transform.localScale = new Vector3(JellyMoveRightDir ? 1 : -1, 1, 1);
        TalkImg.sprite = null;
    }

    public void UpdateJellyHero(float time)
    {
        var Pos = gameObject.transform.position;
        double PrevAngle = GetCenterToBallAngle();

        Vector3 TargetToCenterDir = CenterPos.position - Pos;
        TargetToCenterDir.Normalize();

        var MoveDir = new Vector3();
        Vector3.OrthoNormalize(ref TargetToCenterDir, ref MoveDir);
        MoveDir *= JellyMoveRightDir ? -1 : 1;
        MoveDir.Normalize();

        var TargetMovePos = Pos + (MoveDir * MoveSpeed * time);
        var TargetToCenterDis = Vector3.Distance(CenterPos.position, TargetMovePos);
        Vector3 TargetMoveRealPos = Vector3.MoveTowards(TargetMovePos, TargetToCenterDir, (TargetToCenterDis - CommonData.TRACK_RADIUS_DISTANCE));
        Pos = TargetMoveRealPos;

        double CurrAngle = GetCenterToBallAngle();

        float speed = (StartMoveSpeed * MoveSpeedOffsetPsercent) + StartMoveSpeed;
        if (CurrAngle >= 180 && PrevAngle >= 180)
        {
            MoveSpeed = speed * Mathf.Abs(((float)CurrAngle - 360f)) / 180f + speed * 0.3f;
        }
        else
        {
            MoveSpeed = speed * (float)CurrAngle / 180f + speed * 0.3f;
        }
        Anim.speed = MoveSpeed / StartMoveSpeed;
        gameObject.transform.position = Pos;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, (float)(CurrAngle + 180));
    }

    public void PlusMoveSpeed(float speed)
    {
        MoveSpeed += speed;
    }

    private float GetCenterToBallAngle()
    {
        var Pos = gameObject.transform.position;
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

    public void PlusMoveSpeedOffset(float percent)
    {
        MoveSpeedOffsetPsercent += percent;
        if (MoveSpeedOffsetPsercent < -0.7f)
            MoveSpeedOffsetPsercent = -0.7f;
    }

    public void SetTalkImg(string type)
    {
        StopAllCoroutines();
        if (type == "demage")
            TalkImg.sprite = (Sprite)Resources.Load("talk_demage", typeof(Sprite));
        else if (type == "chest")
            TalkImg.sprite = (Sprite)Resources.Load("talk_chest", typeof(Sprite));

        StartCoroutine(Co_TalkImg());
    }

    IEnumerator Co_TalkImg()
    {
        yield return new WaitForSeconds(0.3f);
        TalkImg.sprite = null;
    }
}
