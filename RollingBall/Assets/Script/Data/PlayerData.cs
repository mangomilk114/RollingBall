using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData
{
    public static PlayerData _instance = null;
    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerData();
            }
            return _instance;
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public int StageIndex = 0;
        public int Score = 0;

        public void Save()
        {
            StageIndex = PlayerData.Instance.StageIndex;
            Score = PlayerData.Instance.Score;
        }

        public void Load()
        {
            PlayerData.Instance.StageIndex = StageIndex;
            PlayerData.Instance.Score = Score;
        }
    }

    private SaveData MySaveData = new SaveData();

    public int StageIndex = 0;
    public int Score = 0;

    public void SaveFile()
    {
        MySaveData.Save();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = pathForDocumentsFile("PlayerData.ini");
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, MySaveData);
        stream.Close();
    }

    public void LoadFile()
    {
        string path = pathForDocumentsFile("PlayerData.ini");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            MySaveData = (SaveData)formatter.Deserialize(stream);
            stream.Close();
            MySaveData.Load();
        }
        else
        {
            StageIndex = 0;
            Score = 0;
            SaveFile();
        }
    }

    public string pathForDocumentsFile(string filename)
    {
#if UNITY_EDITOR
        string path_pc = Application.dataPath;
        path_pc = path_pc.Substring(0, path_pc.LastIndexOf('/'));
        return Path.Combine(path_pc, filename);
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
        path = path.Substring(0, path.LastIndexOf('/'));
        return Path.Combine(path, filename);
#elif UNITY_IOS
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
        path = path.Substring(0, path.LastIndexOf('/'));
        return Path.Combine(Path.Combine(path, "Documents"), filename);
#endif
    }

    public void Initialize()
    {
        LoadFile();
    }

    public void SetStageIndex(int index)
    {
        StageIndex = index;
        SaveFile();
    }
    public void PlusScore(int score)
    {
        Score += score;
        SaveFile();
    }
}