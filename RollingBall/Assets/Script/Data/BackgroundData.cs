using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BackgroundData
{
    public int id;
    public string img;
    public BackgroundData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        img = node.Attributes.GetNamedItem("img").Value;
    }
}