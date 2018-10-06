using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class JellyData
{
    public int id;

    public JellyData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
    }
}