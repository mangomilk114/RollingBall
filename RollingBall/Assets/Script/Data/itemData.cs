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
        itemtype = CommonData.GetItemType(node.Attributes.GetNamedItem("type").Value);
        img = node.Attributes.GetNamedItem("img").Value;
        value = int.Parse(node.Attributes.GetNamedItem("value").Value);
    }

    
}