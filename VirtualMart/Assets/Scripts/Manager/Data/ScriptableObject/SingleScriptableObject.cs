using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static string scriptableObjectPath = "06.Data/ScriptableObject/" + typeof(T).Name;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>(scriptableObjectPath);
            }
            if (instance == null)
            {
                instance = CreateInstance<T>();
            }
            return instance;
        }
    }
}

