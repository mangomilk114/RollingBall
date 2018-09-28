using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEST : MonoBehaviour {

    public GameObject Center;
    public SpriteRenderer Ball;
    public Button SpeedUp;

    private float RadiusDis = 5f;
    private float MoveSpeed = 5f;
    private float ReverseSpeed = 0.1f;
    private bool BallMoveRightDir = true;
    void Awake()
    {
        SpeedUp.onClick.AddListener(OnClickSpeedUp);
    }
    void Start()
    {
        StartCoroutine(CoTargetMove());
    }

    void Update()
    {
        // 센터를 바라보는 방향 백터 만들어주기
        Vector3 TargetToCenterVec3 = Center.transform.position - Ball.gameObject.transform.position;
        TargetToCenterVec3.Normalize();
        Debug.DrawRay(Ball.gameObject.transform.position, TargetToCenterVec3, Color.black);

        // 오른쪽으로 이동하는 방향 백터 만들어주기
        var TargetToRightVec3 = new Vector3();
        Vector3.OrthoNormalize(ref TargetToCenterVec3, ref TargetToRightVec3);
        TargetToRightVec3 *= -1;
        Debug.DrawRay(Ball.gameObject.transform.position, TargetToRightVec3, Color.blue);

        Debug.LogFormat("Speed : {0}", MoveSpeed);
    }

    IEnumerator CoTargetMove()
    {
        while (true)
        {
            double PrevAngle = GetCenterToBallAngle();

            Vector3 TargetToCenterDir = Center.transform.position - Ball.gameObject.transform.position;
            TargetToCenterDir.Normalize();

            var MoveDir = new Vector3();
            Vector3.OrthoNormalize(ref TargetToCenterDir, ref MoveDir);
            MoveDir *= BallMoveRightDir ? -1 : 1;
            MoveDir.Normalize();

            var TargetMovePos = Ball.transform.position + (MoveDir * MoveSpeed * Time.deltaTime);
            var TargetToCenterDis = Vector3.Distance(Center.transform.position, TargetMovePos);
            Vector3 TargetMoveRealPos = Vector3.MoveTowards(TargetMovePos, TargetToCenterDir, (TargetToCenterDis - RadiusDis));
            Ball.transform.position = TargetMoveRealPos;

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
                
            yield return null;
        }
    }

    public float GetCenterToBallAngle()
    {
        float Angle;
        float dX = Center.transform.position.x - Ball.gameObject.transform.position.x;
        float dY = Center.transform.position.y - Ball.gameObject.transform.position.y;

        float dRad = Mathf.Atan2(dY, dX);
        Angle = (float)((dRad * 180) / 3.14159265);
        Angle += 90;

        if (Angle < 0)
            Angle = 360 + Angle;

        return Angle;
    }

    public void OnClickSpeedUp()
    {
        MoveSpeed += 10f;
    }
}
