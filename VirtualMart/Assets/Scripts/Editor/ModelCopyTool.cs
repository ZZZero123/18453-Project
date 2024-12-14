using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelCopyTool : ScriptableObject
{
    [MenuItem("Tools/MyTool/CopyModel")]
    static void CopyModel()
    {
        GameObject target = Selection.activeGameObject;
        GameObject newObj = GameObject.Instantiate(target);
        newObj.transform.position = target.transform.position;
    }
    

}
