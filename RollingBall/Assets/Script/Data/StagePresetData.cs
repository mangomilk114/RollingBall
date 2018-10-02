using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StagePresetData
{
    public int id;
    public Dictionary<string, int> itemTypedic = new Dictionary<string, int>();
    public int itemcount = 0;

    public StagePresetData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        itemcount = 0;
        for (int index_1 = 1; index_1 <= 6; index_1++)
        {
            var data = node.Attributes.GetNamedItem(string.Format("object_{0}", index_1)).Value;
            if(data != "")
            {
                var listStringArr = data.Split(',');
                int count = int.Parse(listStringArr[1]);
                itemTypedic.Add(listStringArr[0], count);
                itemcount += count;
            }
        }
    }
}