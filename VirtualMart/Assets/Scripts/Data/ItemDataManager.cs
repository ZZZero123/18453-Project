using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : SingletonMonoBase<ItemDataManager>
{
    public ItemDataSettings itemDataSettings;
    private Dictionary<ItemType, List<ItemData>> itemDataDic = new Dictionary<ItemType, List<ItemData>>(); //key为模型类型，value为此类型下的所有模型
    protected override void Awake()
    {
        base.Awake();
        InitItemData();
    }
    private void InitItemData()
    {
        if (itemDataSettings == null)
        {
            return;
        }
        List<ItemData> itemDataList = itemDataSettings.itemDataList;
        foreach (ItemData item in itemDataList)
        {
            if (itemDataDic.ContainsKey(item.itemType))
            {
                itemDataDic[item.itemType].Add(item);
            }
            else
            {
                itemDataDic.Add(item.itemType, new List<ItemData>() { item });
            }
        }
    }
    public List<ItemData> GetItemsByType(ItemType itemType)
    {
        if (itemDataDic.ContainsKey(itemType))
        {
            return itemDataDic[itemType];
        }
        else
        {
            return null;
        }
    }
}
