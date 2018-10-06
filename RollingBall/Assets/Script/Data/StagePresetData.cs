using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StagePresetData
{
    public int id;
    private List<KeyValuePair<CommonData.ITEM_TYPE, int>> itemTypeList = new List<KeyValuePair<CommonData.ITEM_TYPE, int>>();

    public StagePresetData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);

        for (int index_1 = 1; index_1 <= 6; index_1++)
        {
            var data = node.Attributes.GetNamedItem(string.Format("object_{0}", index_1)).Value;
            if(data != "")
            {
                var listStringArr = data.Split(',');
                int count = int.Parse(listStringArr[1]);
                itemTypeList.Add(new KeyValuePair<CommonData.ITEM_TYPE, int>(CommonData.GetItemType(listStringArr[0]), count));
            }
        }
    }

    public List<KeyValuePair<CommonData.ITEM_TYPE, int>> GetPresetItemList()
    {
        List<KeyValuePair<CommonData.ITEM_TYPE, int>> returnValue = new List<KeyValuePair<CommonData.ITEM_TYPE, int>>();
        returnValue.InsertRange(0, itemTypeList);

        // TODO 환웅 코인??
        //int coinCraetePercent = Random.Range(0, 101);
        //if (coinCraetePercent < CommonData.COIN_ITEM_CRAETE_PERCENT)
        //    returnValue.Add(new KeyValuePair<string, int>("COIN", Random.Range(0, 2)));

        return returnValue;
    }
}