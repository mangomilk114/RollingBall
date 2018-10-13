using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class PresetData
{
    public CommonData.ITEM_TYPE ItemType = CommonData.ITEM_TYPE.NONE;
    public float Angle = -1;
    public float MoveAngle = 0;

    public PresetData()
    {
        ItemType = CommonData.ITEM_TYPE.NONE;
        Angle = -1;
        MoveAngle = 0;
    }
}

public class StagePresetData
{
    public int id;
    public List<PresetData> itemTypeList = new List<PresetData>();

    public StagePresetData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);

        for (int index_1 = 1; index_1 <= 6; index_1++)
        {
            var data = node.Attributes.GetNamedItem(string.Format("object_{0}", index_1)).Value;
            if(data != "")
            {
                var listStringArr = data.Split(',');
                var itemType = CommonData.GetItemType(listStringArr[0]);
                int count = int.Parse(listStringArr[1]);
                float angle = -1f;
                float moveangle = -1f;
                if (listStringArr.Length >= 3)
                    angle = float.Parse(listStringArr[2]);
                if (listStringArr.Length >= 4)
                    moveangle = float.Parse(listStringArr[3]);

                for (int index_2 = 0; index_2 < count; index_2++)
                {
                    var presetData = new PresetData();
                    presetData.ItemType = itemType;
                    presetData.Angle = angle;
                    presetData.MoveAngle = moveangle;
                    itemTypeList.Add(presetData);
                }
                
            }
        }
    }
}