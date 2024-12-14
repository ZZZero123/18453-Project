using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour
{
    public Transform modelCanvas;
    public MeshRenderer[] renderersToChangeColor;
    [HideInInspector]
    public int materialIndex = -1; //-1Ϊ��ʼ����
    [HideInInspector]
    public ItemType itemType;
    private Material[] originMaterials;
    private InteractableUnityEventWrapper eventWrapper;
    private RayInteractable rayInteractable;
    private Outline outline;
    private BasePanel currentPanel;

    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private int previousMaterialIndex;

    private void Awake()
    {
        eventWrapper = GetComponent<InteractableUnityEventWrapper>();
        rayInteractable = GetComponent<RayInteractable>();
        outline = GetComponent<Outline>();
        if (renderersToChangeColor.Length == 0)
        {
            //��ʼ�����Ըı���ɫ��MeshRenderer
            renderersToChangeColor = GetComponentsInChildren<MeshRenderer>();
        }
        SaveOriginColor();
    }
    private void OnDestroy()
    {
        eventWrapper.WhenHover.RemoveListener(EnableOutline);
        eventWrapper.WhenUnhover.RemoveListener(DisableOutline);
        eventWrapper.WhenSelect.RemoveListener(ShowUIInfo);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlace()
    {
        rayInteractable.enabled = true;
        eventWrapper.WhenHover.AddListener(EnableOutline);
        eventWrapper.WhenUnhover.AddListener(DisableOutline);
        eventWrapper.WhenSelect.AddListener(ShowUIInfo);
    }
    public void ConfirmPlace()
    {
        OnPlace();
        this.TriggerEvent<GameObject, Action>(EventName.CreateAndSaveAnchor, gameObject, null);
    }
    public void UpdateCurrentPanel(BasePanel panel)
    {
        currentPanel = panel;
    }
    public void ResetColor()
    {
        for (int i = 0; i < renderersToChangeColor.Length; i++)
        {
            renderersToChangeColor[i].material = originMaterials[i];
        }
        materialIndex = -1;
    }
    public void SetColor(Material material, int materialIndex)
    {
        for (int i = 0; i < renderersToChangeColor.Length; i++)
        {
            renderersToChangeColor[i].material = material;
        }
        this.materialIndex = materialIndex;
    }
    private void SaveOriginColor()
    {
        if (renderersToChangeColor != null)
        {
            originMaterials = new Material[renderersToChangeColor.Length];
            for (int i = 0; i < renderersToChangeColor.Length; i++)
            {
                originMaterials[i] = renderersToChangeColor[i].material;
            }
        }
    }
    private void ShowUIInfo()
    {
        if (UI3DManager.Instance.IsOperating)
        {
            return;
        }
   
        if(currentPanel == null)
        {
            UI3DManager.Instance.ShowPanelOnSpecificCanvas<ModeSelectionPanel>(nameof(ModeSelectionPanel), modelCanvas, (panel) =>
            {
                currentPanel = panel;
                previousPosition = transform.position;
                previousRotation = transform.rotation;
                previousMaterialIndex = materialIndex;
            });
        }
        else
        {
           
            if (!currentPanel.gameObject.activeInHierarchy)
            {
                UI3DManager.Instance.ShowSpecificPanel(currentPanel, (panel) =>
                {
                    previousPosition = transform.position;
                    previousRotation = transform.rotation;
                    previousMaterialIndex = materialIndex;
                });
            }
            
            else
            {
                UI3DManager.Instance.HideSpecificPanel(currentPanel);
                TryCreateAndSaveAnchorWhenHideUI();
            }
        }
    }
    private void TryCreateAndSaveAnchorWhenHideUI()
    {
        
        if (CheckIfItemStateChanged())
        {
            
            if (transform.GetComponentInParent<OVRSpatialAnchor>() == null)
            {
                this.TriggerEvent<GameObject, Action>(EventName.CreateAndSaveAnchor, gameObject, null);
            }
            else
            {
                
                this.TriggerEvent<GameObject, Action>(EventName.DeleteAnchor, gameObject, () =>
                {
                    this.TriggerEvent<GameObject, Action>(EventName.CreateAndSaveAnchor, gameObject, null);
                });
            }


        }
    }
    private bool CheckIfItemStateChanged()
    {
        float positionTolerance = 0.1f; 
        float rotationTolerance = 0.1f; 

        bool positionChanged = Vector3.Distance(transform.position, previousPosition) > positionTolerance;

        bool rotationChanged = Quaternion.Angle(transform.rotation, previousRotation) > rotationTolerance;

        bool materialChanged = materialIndex != previousMaterialIndex;

        if (positionChanged || rotationChanged || materialChanged)
        {
            return true;
        }
        return false;
    }
    private void EnableOutline()
    {
        outline.enabled = true;
    }
    private void DisableOutline()
    {
        outline.enabled = false;
    }
}
