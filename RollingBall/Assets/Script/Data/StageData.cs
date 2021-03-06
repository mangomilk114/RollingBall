﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StageData
{
    public int id;
    public float start_speed;
    public bool start_rightdir = true;
    public int preset;

    public StageData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        start_speed = float.Parse(node.Attributes.GetNamedItem("start_speed").Value);
        start_rightdir = bool.Parse(node.Attributes.GetNamedItem("start_rightdir").Value);
        preset = int.Parse(node.Attributes.GetNamedItem("preset").Value);
    }
}