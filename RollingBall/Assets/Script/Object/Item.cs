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

    public virtual void ResetItem()
    {
        Type = CommonData.OBJECT_TYPE.ITEM;
        gameObject.SetActive(false);
        ItemType = CommonData.ITEM_TYPE.NONE;
        UniqueIndex = -1;
        Anim.Rebind();
    }

    public void SetData(CommonData.ITEM_TYPE type, int uniqueIndex)
    {
        gameObject.SetActive(true);
        UniqueIndex = uniqueIndex;
        Data = DataManager.Instance.ItemDataDic[type];
        ItemType = Data.itemtype;

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
            default:
                break;
        }
    }
}