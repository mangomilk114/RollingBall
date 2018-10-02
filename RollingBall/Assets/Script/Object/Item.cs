using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InGameObject
{
    public string ItemName = "";
    public CommonData.ITEM_TYPE ItemType = CommonData.ITEM_TYPE.NONE;
    public SpriteRenderer Img;
    public int UniqueIndex = -1;
    public ItemData Data;

    public void ResetItem()
    {
        Type = CommonData.OBJECT_TYPE.ITEM;
        gameObject.SetActive(false);
        ItemType = CommonData.ITEM_TYPE.NONE;
        Img.sprite = null;
        UniqueIndex = -1;
        ItemName = "";
    }

    public void SetData(string name, int uniqueIndex)
    {
        gameObject.SetActive(true);
        UniqueIndex = uniqueIndex;
        Data = DataManager.Instance.ItemDataDic[name];
        ItemName = Data.name;

        ItemType = Data.itemtype;
        Img.sprite = (Sprite)Resources.Load(Data.img, typeof(Sprite));
    }
}