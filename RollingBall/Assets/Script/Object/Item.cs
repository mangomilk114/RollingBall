using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : CollisionObject
{
    // TODO 환웅 아이템 오브젝트 풀
    public CommonData.ITEM_TYPE ItemType = CommonData.ITEM_TYPE.NONE;
    public SpriteRenderer Img;
    public BoxCollider2D Collider;

    private ItemData Data;

    public void ResetItem()
    {
        gameObject.SetActive(false);
        ItemType = CommonData.ITEM_TYPE.NONE;
        Img.sprite = null;
        Collider.enabled = false;
    }

    public void SetData(CommonData.ITEM_TYPE type)
    {
        gameObject.SetActive(true);
        Collider.enabled = true;
        Data = DataManager.Instance.ItemDataDic[type];

        ItemType = Data.itemtype;
        Img.sprite = (Sprite)Resources.Load(Data.img, typeof(Sprite));
    }
}