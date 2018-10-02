using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemData
{
    public int id;
    public string name;
    public CommonData.ITEM_TYPE itemtype;
    public string img;
    public int value;

    public ItemData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        SetItemType(node.Attributes.GetNamedItem("type").Value);
        img = node.Attributes.GetNamedItem("img").Value;
        value = int.Parse(node.Attributes.GetNamedItem("value").Value);
    }

    private void SetItemType(string type)
    {
        switch(type)
        {
            case "STAR":
                itemtype = CommonData.ITEM_TYPE.STAR;
                break;
            case "SPEED_UP":
                itemtype = CommonData.ITEM_TYPE.SPEED_UP;
                break;
            case "SPEED_DOWN":
                itemtype = CommonData.ITEM_TYPE.SPEED_DOWN;
                break;
            case "BOMB":
                itemtype = CommonData.ITEM_TYPE.BOMB;
                break;
            case "COIN":
                itemtype = CommonData.ITEM_TYPE.COIN;
                break;
            case "POTION":
                itemtype = CommonData.ITEM_TYPE.POTION;
                break;
            default:
                itemtype = CommonData.ITEM_TYPE.NONE;
                break;
        }
    }
}