using Meta.XR.BuildingBlocks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorHandler : MonoBehaviour
{
    public GameObject anchorPrefab;
    private SpatialAnchorCoreBuildingBlock spatialAnchorCore;
    private GameObject currentObjToAnchor; 
    private Dictionary<Guid, string> anchorPrefabDic = new Dictionary<Guid, string>();//key:ê��ID��value:ê��󶨵�ģ�͵�����
    private List<AnchorData> anchorDataList = new List<AnchorData>();

    private Action customAnchorCreateSucessCallback;
    private Action customAnchorDeleteSucessCallback;

    private void Awake()
    {
        spatialAnchorCore = GetComponent<SpatialAnchorCoreBuildingBlock>();
        spatialAnchorCore.OnAnchorCreateCompleted.AddListener(OnAnchorCreateCompleted);
        spatialAnchorCore.OnAnchorsLoadCompleted.AddListener(OnAnchorLoadCompleted);
        EventCenter.Instance.AddListener<GameObject, Action>(EventName.CreateAndSaveAnchor, CreateAnchorOnObj);
        EventCenter.Instance.AddListener<GameObject, Action>(EventName.DeleteAnchor, DeleteAnchor);
        EventCenter.Instance.AddListener(EventName.LoadAnchors, LoadAnchors);
        EventCenter.Instance.AddListener<GameObject>(EventName.DeleteAnchorWithObj, DeleteAnchorWithObj);
        EventCenter.Instance.AddListener(EventName.DeleteAllAnchorsWithObjs, DeleteAllAnchors);
    }



    private void OnDestroy()
    {
        spatialAnchorCore.OnAnchorCreateCompleted.RemoveListener(OnAnchorCreateCompleted);
        spatialAnchorCore.OnAnchorsLoadCompleted.RemoveListener(OnAnchorLoadCompleted);
        EventCenter.Instance.RemoveListener<GameObject, Action>(EventName.CreateAndSaveAnchor, CreateAnchorOnObj);
        EventCenter.Instance.RemoveListener<GameObject, Action>(EventName.DeleteAnchor, DeleteAnchor);
        EventCenter.Instance.RemoveListener(EventName.LoadAnchors, LoadAnchors);
        EventCenter.Instance.RemoveListener<GameObject>(EventName.DeleteAnchorWithObj, DeleteAnchorWithObj);
        EventCenter.Instance.RemoveListener(EventName.DeleteAllAnchorsWithObjs, DeleteAllAnchors);
    }
    private void OnAnchorCreateCompleted(OVRSpatialAnchor anchor, OVRSpatialAnchor.OperationResult result)
    {
        if (result == OVRSpatialAnchor.OperationResult.Success && currentObjToAnchor != null)
        {
            currentObjToAnchor.transform.SetParent(anchor.transform);
            ItemInteractable currentItemInteractable = currentObjToAnchor.GetComponent<ItemInteractable>();
            AnchorData anchorData = new AnchorData()
            {
                anchorId = anchor.Uuid.ToString(),
                anchorObjName = currentObjToAnchor.name,
                itemType = currentItemInteractable.itemType.ToString(),
                materialIndex = currentItemInteractable.materialIndex
            };
            anchorDataList.Add(anchorData);
            SaveAnchor();
            currentObjToAnchor = null;
            customAnchorCreateSucessCallback?.Invoke();
            customAnchorCreateSucessCallback = null;
        }

    }
    private void OnAnchorLoadCompleted(List<OVRSpatialAnchor> anchorList)
    {
        foreach (OVRSpatialAnchor anchor in anchorList)
        {
            if (anchorPrefabDic.TryGetValue(anchor.Uuid, out string objName))
            {
                ResourceManager.Instance.LoadAsync<GameObject>(objName, (spawnedObj) =>
                {
                    spawnedObj.name = objName; 
                    spawnedObj.transform.position = anchor.gameObject.transform.position;
                    spawnedObj.transform.rotation = anchor.gameObject.transform.rotation;
                    spawnedObj.transform.SetParent(anchor.gameObject.transform);  


                    ItemInteractable itemInteractable = spawnedObj.GetComponent<ItemInteractable>();
                    if (itemInteractable != null)
                    {
                        ColorChoice colorChoice = spawnedObj.GetComponentInChildren<ColorChoice>();
                        // AnchorData
                        AnchorData anchorData = anchorDataList.Find(data => data.anchorId == anchor.Uuid.ToString());
                        int materialIndex = anchorData.materialIndex;
                        if (materialIndex != -1)
                        {
                            Material material = colorChoice.materialElements[anchorData.materialIndex].material;
                            itemInteractable.SetColor(material, materialIndex);
                        }
                        itemInteractable.itemType = (ItemType)Enum.Parse(typeof(ItemType), anchorData.itemType);
                        itemInteractable.OnPlace();

                    }
                });
            }
        }
    }
    private void SaveAnchor()
    {
        JsonManager.Instance.SaveData(anchorDataList, "Anchors");

    }
    /// <summary>
    /// ����ê��
    /// </summary>
    /// <param name="objToAnchor"></param>
    public void CreateAnchorOnObj(GameObject objToAnchor, Action callback)
    {
        Vector3 spawnPosition = objToAnchor.transform.position;
        Quaternion spawnRotation = objToAnchor.transform.rotation;
        currentObjToAnchor = objToAnchor;
        spatialAnchorCore.InstantiateSpatialAnchor(anchorPrefab, spawnPosition, spawnRotation);
        customAnchorCreateSucessCallback = callback;
    }
    /// <summary>
    /// </summary>
    public void LoadAnchors()
    {
        print("����ê��");
        anchorDataList = JsonManager.Instance.LoadData<List<AnchorData>>("Anchors");
        List<Guid> uuidList = new List<Guid>();
        foreach (AnchorData anchorData in anchorDataList)
        {
            if (Guid.TryParse(anchorData.anchorId, out Guid id))
            {
                uuidList.Add(id);
                anchorPrefabDic[id] = anchorData.anchorObjName;
            }
        }
        spatialAnchorCore.LoadAndInstantiateAnchors(anchorPrefab, uuidList);
    }
    /// <summary>
    /// </summary>
    /// <param name="anchorObj"></param>
    public void DeleteAnchor(GameObject objToAnchor, Action callback = null)
    {
        Transform anchorObj = objToAnchor.transform.parent;
        if (anchorObj != null)
        {
            OVRSpatialAnchor anchor = anchorObj.GetComponent<OVRSpatialAnchor>();
            if (anchor != null)
            {
                objToAnchor.transform.SetParent(null);
                DeleteSpecificAnchorAsync(anchor);
                customAnchorDeleteSucessCallback = callback;
            }
        }


    }
    public void DeleteAnchorWithObj(GameObject objToAnchor)
    {
        DeleteAnchor(objToAnchor, () =>
        {
            Destroy(objToAnchor);
        });

    }
    public void DeleteAllAnchors()
    {
        spatialAnchorCore.EraseAllAnchors();
        JsonManager.Instance.DeleteData("Anchors");
    }
    private async void DeleteSpecificAnchorAsync(OVRSpatialAnchor anchor)
    {
        var result = await anchor.EraseAnchorAsync();
        if (result.Success)
        {
            anchorDataList.RemoveAll(data => data.anchorId == anchor.Uuid.ToString());
            SaveAnchor();
            Destroy(anchor.gameObject);
            customAnchorDeleteSucessCallback?.Invoke();
            customAnchorDeleteSucessCallback = null;
        }
    }
}
public struct AnchorData
{
    public string anchorId;
    public string anchorObjName;
    public string itemType;
    public int materialIndex; 
}


