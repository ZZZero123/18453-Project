using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAttachedUIPanel : BaseFadePanel
{
    protected ItemInteractable itemInteractable;
    protected GameObject attachedModel;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        itemInteractable = transform.GetComponentInParent<ItemInteractable>();
        attachedModel = itemInteractable.gameObject;
    }

    
}
