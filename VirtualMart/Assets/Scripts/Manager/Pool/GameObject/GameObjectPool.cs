using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : ObjectPoolBase<GameObject>
{
    public GameObject fatherObj;
    /// <summary>
    /// </summary>
    /// <param name="poolObj"></param>
    /// <param name="gameObjectRoot"></param>
    public GameObjectPool(GameObject poolObj, GameObject gameObjectRoot)
    {
        fatherObj = new GameObject(poolObj.name);
        fatherObj.transform.parent = gameObjectRoot.transform;
        itemPool = new SingleLinkedList<GameObject>();
        PushItem(poolObj);
    }
    public override GameObject GetItem()
    {
        var obj = itemPool.First.Value;
        itemPool.DeleteFirst();
        obj.transform.SetParent(null,false);
        obj.SetActive(true);
        return obj;
    }
    public override void PushItem(GameObject item)
    {
        itemPool.AddLast(item);
        item.SetActive(false);
        item.transform.SetParent(fatherObj.transform,false);
    }
}
