using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StageData
{
    public int Id;
    public string TrackImg;
    public float StartSpeed;
    public int StarCount;
    public bool StartRightDir = true;
    // TODO 환웅 아이템 타입별로 갯수

    public StageData(XmlNode node)
    {
        Id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        TrackImg = node.Attributes.GetNamedItem("track_img").Value;
        StartSpeed = float.Parse(node.Attributes.GetNamedItem("start_speed").Value);
        StarCount = int.Parse(node.Attributes.GetNamedItem("star").Value);
        StartRightDir = bool.Parse(node.Attributes.GetNamedItem("start_rightdir").Value);
    }
}