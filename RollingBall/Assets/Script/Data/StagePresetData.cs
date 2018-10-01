using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StagePresetData
{
    public int id;
    public int star;
    public int speed_up;
    public int speed_down;
    public int bomb;
    public int coin;
    public int potion;
    public Dictionary<CommonData.ITEM_TYPE, int> itemTypedic = new Dictionary<CommonData.ITEM_TYPE, int>();
    public int itemcount = 0;

    public StagePresetData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        star = int.Parse(node.Attributes.GetNamedItem("star").Value);
        speed_up = int.Parse(node.Attributes.GetNamedItem("speed_up").Value);
        speed_down = int.Parse(node.Attributes.GetNamedItem("speed_down").Value);
        bomb = int.Parse(node.Attributes.GetNamedItem("bomb").Value);
        coin = int.Parse(node.Attributes.GetNamedItem("coin").Value);
        potion = int.Parse(node.Attributes.GetNamedItem("potion").Value);

        itemTypedic.Add(CommonData.ITEM_TYPE.STAR, star);
        itemcount += star;
        itemTypedic.Add(CommonData.ITEM_TYPE.SPEED_UP, speed_up);
        itemcount += speed_up;
        itemTypedic.Add(CommonData.ITEM_TYPE.SPEED_DOWN, speed_down);
        itemcount += speed_down;
        itemTypedic.Add(CommonData.ITEM_TYPE.POTION, potion);
        itemcount += potion;
        itemTypedic.Add(CommonData.ITEM_TYPE.COIN, coin);
        itemcount += coin;
        itemTypedic.Add(CommonData.ITEM_TYPE.BOMB, bomb);
        itemcount += bomb;

    }
}