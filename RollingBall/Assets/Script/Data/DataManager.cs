﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class DataManager
{
    public static DataManager _instance = null;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }
    }

    public Dictionary<int, JellyData> JellyHeroDataDic = new Dictionary<int, JellyData>();
    public List<StageData> StageDataList = new List<StageData>();
    public Dictionary<int, StagePresetData> StagePresetDataDic = new Dictionary<int, StagePresetData>();
    public Dictionary<int, BackgroundData> BackgroundDataDic = new Dictionary<int, BackgroundData>();
    public Dictionary<CommonData.ITEM_TYPE, ItemData> ItemDataDic = new Dictionary<CommonData.ITEM_TYPE, ItemData>();
    private List<KeyValuePair<string, string>> LoadingDataXmlList = new List<KeyValuePair<string, string>>();

    public IEnumerator LoadingData()
    {
        if (LoadingDataXmlList.Count <= 0)
        {
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Jelly", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Stage", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Item", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("StagePreset", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Background", "Datas"));
        }

        for (int i = 0; i < LoadingDataXmlList.Count; i++)
        {
            string xmlName = LoadingDataXmlList[i].Key;
            XmlNodeList list = GetXmlNodeList(LoadingDataXmlList[i].Key, LoadingDataXmlList[i].Value);


            if (xmlName == "Jelly")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new JellyData(child);
                        JellyHeroDataDic.Add(data.id, data);
                    }
                }
            }
            else if (xmlName == "Stage")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new StageData(child);
                        StageDataList.Add(data);
                    }
                }
            }
            else if (xmlName == "Item")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new ItemData(child);
                        ItemDataDic.Add(data.itemtype, data);
                    }
                }
            }
            else if (xmlName == "StagePreset")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new StagePresetData(child);
                        StagePresetDataDic.Add(data.id, data);
                    }
                }
            }

            else if (xmlName == "Background")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new BackgroundData(child);
                        BackgroundDataDic.Add(data.id, data);
                    }
                }
            }

            yield return null;
        }
    }

    public XmlNodeList GetXmlNodeList(string fileName, string key)
    {
        TextAsset txtAsset = (TextAsset)Resources.Load("xml/" + fileName);
        XmlDocument xmlDoc = new XmlDocument();
        Debug.Log(txtAsset.text);
        xmlDoc.LoadXml(txtAsset.text);
        XmlNodeList all_nodes = xmlDoc.SelectNodes(key);
        return all_nodes;
    }
}
