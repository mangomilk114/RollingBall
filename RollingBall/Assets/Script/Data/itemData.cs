using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemData
{
    public int Id;
    public CommonData.ITEM_TYPE ItemType;
    public string Img;

    public ItemData(XmlNode node)
    {
        Id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        SetItemType(node.Attributes.GetNamedItem("type").Value);
        Img = node.Attributes.GetNamedItem("img").Value;
    }

    private void SetItemType(string type)
    {
        switch(type)
        {
            case "STAR":
                ItemType = CommonData.ITEM_TYPE.STAR;
                break;
            default:
                ItemType = CommonData.ITEM_TYPE.NONE;
                break;
        }
    }
}