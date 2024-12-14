using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

/// <summary>
/// </summary>
public enum JsonType
{
    JsonUtlity,
    LitJson,
    NewtonsoftJson
}

/// <summary>
/// </summary>
public class JsonManager : SingletonBase<JsonManager>
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public void SaveData<T>(T data, string fileName, bool isSaveInPersistentDataPath = true, JsonType type = JsonType.NewtonsoftJson) where T : new()
    {
        string path = null;
        if (isSaveInPersistentDataPath)
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        else
        {
            path = Application.streamingAssetsPath + "/" + fileName + ".json";
        }
        
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                //jsonStr = JsonMapper.ToJson(data);
                break;
            case JsonType.NewtonsoftJson:
                jsonStr = JsonConvert.SerializeObject(data); Debug.Log(jsonStr);
                break;
        }
        File.WriteAllText(path, jsonStr);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public T LoadData<T>(string fileName, bool isSaveInPersistentDataPath = true, JsonType type = JsonType.NewtonsoftJson) where T : new()
    {
        string jsonStr = null;
        string path = null;
        if (isSaveInPersistentDataPath)
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        else
        {
            path = Application.streamingAssetsPath + "/" + fileName + ".json";
        }
        if (!File.Exists(path))
        {
            return new T();
        }

        jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                //data = JsonMapper.ToObject<T>(jsonStr);
                break;
            case JsonType.NewtonsoftJson:
                data = JsonConvert.DeserializeObject<T>(jsonStr);
                break;
        }
        return data;
    }
    private T JsonToObj<T>(string jsonStr, JsonType type = JsonType.NewtonsoftJson) where T:new()
    {
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                //data = JsonMapper.ToObject<T>(jsonStr);
                break;
            case JsonType.NewtonsoftJson:
                data = JsonConvert.DeserializeObject<T>(jsonStr);
                break;
        }
        return data;
    }
    public void LoadDataFromAB<T>(string fileName,Action<T> callback, JsonType type = JsonType.NewtonsoftJson) where T : new()
    {
        ResourceManager.Instance.LoadAsync<TextAsset>(fileName, (textAsset) =>
        {
            T data = JsonToObj<T>(textAsset.text, type);
            callback?.Invoke(data);
        });
    }
    public void DeleteData(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path))
        {
            Debug.Log($"cannot find {path} or not saved");
            return;
        }
        File.Delete(path);
    }
}

