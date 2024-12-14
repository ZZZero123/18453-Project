using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ItemData 
{
    public string itemName;
    public ItemType itemType;
    public MRUKAnchor.SceneLabels sceneLabels = ~(MRUKAnchor.SceneLabels)0;
}

public enum ItemType
{
    Furniture,
    WallDecoration
}
