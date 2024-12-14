using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/Item Data", order = 0)]
public class ItemDataSettings : ScriptableObject
{
    public List<ItemData> itemDataList;
}
