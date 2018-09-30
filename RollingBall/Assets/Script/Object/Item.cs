using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : CollisionObject
{
    // TODO 환웅 아이템 오브젝트 풀
    public CommonData.ITEM_TYPE ItemType = CommonData.ITEM_TYPE.NONE;
    public SpriteRenderer Img;

    private ItemData Data;

    public void SetData(CommonData.ITEM_TYPE type)
    {
        Data = DataManager.Instance.ItemDataDic[type];

        ItemType = Data.ItemType;
        Img.sprite = (Sprite)Resources.Load(Data.Img, typeof(Sprite));
    }
}