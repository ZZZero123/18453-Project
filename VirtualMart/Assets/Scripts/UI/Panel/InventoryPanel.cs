using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : BaseFadePanel
{

    public Color toggleOnColor;
    public Color toggleOffColor;
    public Toggle[] itemTypeGroup;
    public Transform inventoryParent;
    public int pageMaxNum = 8;
    public TextMeshProUGUI pageNum; 
    public GameObject itemCellPrefab;
    public ItemType defaultItemType = ItemType.Furniture;
    private Dictionary<ItemType, PageInfo<ItemData>> itemDataDic = new Dictionary<ItemType, PageInfo<ItemData>>();
    private ItemType currentItemType;
    private Transform canvas;
    private UIMenuDelayFollowHead uiFollow;
    private RayInteractable rayInteractable;
    private int totalPageNum;
    private int startIndex; 
    private int endIndex;
    private int loadedModelCount; 
    private int totalModelCount;
    private bool isAnchored;
    private enum PageDirection { Previous, Next }
    private void Start()
    {
        canvas = transform.parent;
        uiFollow = canvas.GetComponent<UIMenuDelayFollowHead>();
        rayInteractable = canvas.GetComponentInChildren<RayInteractable>();
        ShowDefaultPage();
    }
    private void ShowDefaultPage()
    {
        Toggle defaultToggle = GetUIComponentByName<Toggle>(defaultItemType.ToString());
        Image image = defaultToggle.GetComponent<Image>();
        image.color = toggleOnColor;
        InitItemCells(defaultItemType);
    }
    private void DeleteCurrentItemCells()
    {
        if (inventoryParent.childCount == 0)
        {
            return;
        }
        for (int i = 0; i < inventoryParent.childCount; i++)
        {
            Destroy(inventoryParent.GetChild(i).gameObject);
        }
        loadedModelCount = 0;
        totalModelCount = 0;
    }
    public void ControlRayInteractable(bool isEnable)
    {
        if (rayInteractable != null)
        {
            rayInteractable.enabled = isEnable;
        }
    }
    public void InitItemCells(ItemType itemType)
    {
        DeleteCurrentItemCells();
        currentItemType = itemType;
        if (!itemDataDic.ContainsKey(itemType))
        {
            List<ItemData> list = ItemDataManager.Instance.GetItemsByType(itemType);
            if (list == null) return;
            itemDataDic.Add(itemType, new PageInfo<ItemData>() { currentPage = 1, everyPageMax = pageMaxNum, pageItems = list });
        }
        PageInfo<ItemData> pageInfo = itemDataDic[itemType];
        startIndex = pageInfo.everyPageMax * (pageInfo.currentPage - 1); 
        endIndex = pageInfo.everyPageMax * pageInfo.currentPage - 1;
        if (pageInfo.pageItems.Count - 1 < endIndex)
        {
            endIndex = pageInfo.pageItems.Count - 1;
        }
 
        totalPageNum = (pageInfo.pageItems.Count - 1) / pageMaxNum + 1; 
        totalModelCount = endIndex - startIndex + 1;
        pageNum.text = $"{pageInfo.currentPage}/{totalPageNum}"; 
                                                                
        if (!isAnchored)
        {

            uiFollow.ControlFollow(false);
        }
        List<ItemCell> itemCells = new List<ItemCell>();
        List<ItemData> itemDataList = new List<ItemData>();
        for (int i = startIndex; i <= endIndex; i++)
        {
            ItemData itemData = pageInfo.pageItems[i];
            GameObject itemCellObj = Instantiate(itemCellPrefab);
            itemCellObj.name = itemData.itemName;
            itemCellObj.transform.SetParent(inventoryParent, false);
            ItemCell itemCell = itemCellObj.GetComponent<ItemCell>();
            if (!isAnchored)
            {
                itemCell.onLoadedCompleted.AddListener(OnItemModelLoaded);
            }
            itemCells.Add(itemCell);
            itemDataList.Add(itemData);

        }
        StartCoroutine(InitItemCell(itemCells, itemDataList));
    }

    private IEnumerator InitItemCell(List<ItemCell> itemCells, List<ItemData> itemDataList)
    {
        yield return null; 
        for (int i = 0; i < itemCells.Count; i++)
        {
            ItemCell itemCell = itemCells[i];
            ItemData itemData = itemDataList[i];
            yield return StartCoroutine(itemCell.InitCellInfo(itemData));
        }
    }

    private void OnItemModelLoaded()
    {
        loadedModelCount++;

        if (loadedModelCount == totalModelCount)
        {
            uiFollow.ControlFollow(true);
        }
    }
    protected override void OnToggleValueChanged(bool value, Toggle toggle)
    {
        base.OnToggleValueChanged(value, toggle);
        if (toggle.name == "ê��")
        {
            Image image = toggle.GetComponent<Image>();
            if (image != null)
            {
                if (value)
                {
                    image.color = toggleOnColor;
                }
                else
                {
                    image.color = toggleOffColor;
                }
            }
            AnchorPanel(value);
        }
        else
        {
            OnItemTypeToggleChanged(toggle);
            InitItemCells((ItemType)Enum.Parse(typeof(ItemType), toggle.name));
        }
    }
    private void OnItemTypeToggleChanged(Toggle changedToggle)
    {
        foreach (Toggle toggle in itemTypeGroup)
        {
            Image image = toggle.GetComponent<Image>();
            if (toggle == changedToggle)
            {
                image.color = toggleOnColor;
            }
            else
            {
                image.color = toggleOffColor;
            }
        }
    }
    private void AnchorPanel(bool needAnchored)
    {
        uiFollow.ControlFollow(!needAnchored);
        isAnchored = needAnchored;
    }
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "NextPage":
                TurnPage(PageDirection.Next);
                break;
            case "PreviousPage":
                TurnPage(PageDirection.Previous);
                break;
            case "ClearObjects":
                this.TriggerEvent(EventName.DeleteAllAnchorsWithObjs);
                break;
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="direction"></param>
    private void TurnPage(PageDirection direction)
    {
        PageInfo<ItemData> pageInfo = itemDataDic[currentItemType];

        if (direction == PageDirection.Previous && pageInfo.currentPage <= 1)
        {
            return;
        }
        if (direction == PageDirection.Next && endIndex == pageInfo.pageItems.Count - 1)
        {
            return;
        }

        if (direction == PageDirection.Previous)
        {
            pageInfo.currentPage--;
        }
        else
        {
            pageInfo.currentPage++;
        }
        InitItemCells(currentItemType);
    }

}
