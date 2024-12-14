using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemCell : BaseUIPanel
{
    public Transform modelContainer;
    public UnityEvent onLoadedCompleted;
    private ItemData itemData;

    private SphereCollider sphereCollider;
    protected override void Awake()
    {
        base.Awake();
        sphereCollider = modelContainer.GetComponent<SphereCollider>();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        onLoadedCompleted.RemoveAllListeners();
    }
    

    public IEnumerator InitCellInfo(ItemData itemData)
    {
        yield return null;
        this.itemData = itemData;
        bool isLoadingCompleted = false;
        ResourceManager.Instance.LoadAsync<GameObject>(itemData.itemName + "-Origin", (item) =>
        {
            item.name = itemData.itemName;
            item.transform.SetParent(modelContainer);
            Vector3 targetScale = ScaleUtil.ScaleToFit(item, sphereCollider.bounds);
            item.transform.localScale = new Vector3(item.transform.localScale.x * targetScale.x, item.transform.localScale.y * targetScale.y, item.transform.localScale.z * targetScale.z);
            item.transform.rotation = modelContainer.rotation;
            item.transform.position = new Vector3(modelContainer.position.x, sphereCollider.bounds.min.y, modelContainer.position.z);//ģ�͵�ԭ���ڵײ�������Ҫ����ģ���ڸ����е�λ��
            onLoadedCompleted?.Invoke();
            isLoadingCompleted = true;
        });
        yield return new WaitUntil(() => isLoadingCompleted);
    }
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        if(modelContainer.childCount > 0)
        {
            this.TriggerEvent(EventName.StartPlaceObj, itemData);
        }
    }
}
