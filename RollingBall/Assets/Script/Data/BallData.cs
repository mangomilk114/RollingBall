using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BallData
{
    public int id;
    public string ball_img;

    public BallData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        ball_img = node.Attributes.GetNamedItem("ball_img").Value;
    }
}