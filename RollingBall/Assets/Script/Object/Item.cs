using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InGameObject
{
    [System.NonSerialized]
    public CommonData.ITEM_TYPE ItemType = CommonData.ITEM_TYPE.NONE;
    [System.NonSerialized]
    public int UniqueIndex = -1;
    [System.NonSerialized]
    public ItemData Data;
    public Animator Anim;

    private Transform CenterPos;
    private float MoveSpeed = 0.5f;
    private float MoveDegree = 0;
    private float FirtstDegree = 0;
    private float MoveDegreeMin = 0;
    private float MoveDegreeMax = 0;
    private float MoveTempDegree = 0;
    private bool MoveRightDir = false;

    public virtual void ResetItem()
    {
        Type = CommonData.OBJECT_TYPE.ITEM;
        gameObject.SetActive(false);
        ItemType = CommonData.ITEM_TYPE.NONE;
        UniqueIndex = -1;
        Anim.Rebind();
    }

    public void SetData(CommonData.ITEM_TYPE type, int uniqueIndex, float moveDegree = 20)
    {
        gameObject.SetActive(true);
        UniqueIndex = uniqueIndex;
        Data = DataManager.Instance.ItemDataDic[type];
        ItemType = Data.itemtype;
        FirtstDegree = Degree;
        MoveDegree = moveDegree;
        MoveDegreeMax = (FirtstDegree + MoveDegree);
        MoveDegreeMin = (FirtstDegree - MoveDegree);
        MoveTempDegree = FirtstDegree;


        switch (type)
        {
            case CommonData.ITEM_TYPE.NONE:
                break;
            case CommonData.ITEM_TYPE.CHEST:
                Anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/Chest/Chest", typeof(RuntimeAnimatorController));
                break;
            case CommonData.ITEM_TYPE.SAW:
                Anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/Saw/Saw", typeof(RuntimeAnimatorController));
                break;
            case CommonData.ITEM_TYPE.SPEED_DOWN:
                Anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/SpeedDown/SpeedDown", typeof(RuntimeAnimatorController));
                break;
            case CommonData.ITEM_TYPE.SPEED_UP:
                Anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/SpeedUp/SpeedUp", typeof(RuntimeAnimatorController));
                break;
            default:
                break;
        }
    }

    public void UpdateItem(float time)
    {
        if(MoveDegree != 0)
        {
            MoveTempDegree = MoveTempDegree + (MoveRightDir ? MoveSpeed : -MoveSpeed);
            if(MoveTempDegree < 0)
                SetPlace(360f - MoveTempDegree);
            else
                SetPlace(MoveTempDegree);


            if (MoveRightDir && MoveTempDegree > MoveDegreeMax ||
                MoveRightDir == false && MoveTempDegree < MoveDegreeMin)
                MoveRightDir = !MoveRightDir;
        }
            
    }

}