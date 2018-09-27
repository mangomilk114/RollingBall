using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

    public GameObject Center;
    public SpriteRenderer Ball;

    private float RadiusDis = 5f;
    private float UpSpeed = 5f;
    private float DownSpeed = 0f;
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
    }

    IEnumerator CoTargetMove()
    {
        while (true)
        {
            Vector3 TargetToCenterDir = Center.transform.position - Ball.gameObject.transform.position;
            TargetToCenterDir.Normalize();

            var MoveDir = new Vector3();
            Vector3.OrthoNormalize(ref TargetToCenterDir, ref MoveDir);
            MoveDir *= -1;
            MoveDir.Normalize();

            var TargetMovePos = Ball.transform.position + (MoveDir * UpSpeed * Time.deltaTime);
            var TargetToCenterDis = Vector3.Distance(Center.transform.position, TargetMovePos);
            Vector3 TargetMoveRealPos = Vector3.MoveTowards(TargetMovePos, TargetToCenterDir, (TargetToCenterDis - RadiusDis));
            Ball.transform.position = TargetMoveRealPos;

            yield return null;
        }
    }
}
