using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BallData
{
    public int Id;
    public string BallImg;

    public BallData(XmlNode node)
    {
        Id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        BallImg = node.Attributes.GetNamedItem("ball_img").Value;
    }
}