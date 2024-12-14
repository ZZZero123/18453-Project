using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformEditPanel : ModelAttachedUIPanel
{
    public Color toggleOnColor;
    public Color toggleOffColor;
    private Toggle[] toggles;
    protected override void Awake()
    {
        base.Awake();
        EventCenter.Instance.AddListener<Toggle>(EventName.ResetModification, ResetModification);
        EventCenter.Instance.AddListener(EventName.ResetModification, ResetAllToggles);
        toggles = GetComponentsInChildren<Toggle>();
    }
    private void OnDisable()
    {
        ResetAllToggles();
        this.TriggerEvent(EventName.ModifyModelRotation, attachedModel, false);
        this.TriggerEvent(EventName.ModifyModelPosition, attachedModel, false);
    }
    private void ResetAllToggles()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
            Image image = toggle.GetComponent<Image>();
            if (image != null)
            {
                image.color = toggleOffColor;
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventCenter.Instance.RemoveListener<Toggle>(EventName.ResetModification, ResetModification);
        EventCenter.Instance.RemoveListener(EventName.ResetModification, ResetAllToggles);
    }
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Return":
                Return();
                break;
            case "DeleteModel":
                DeleteModel();
                break;
        }
    }
    protected override void OnToggleValueChanged(bool value, Toggle toggle)
    {
        base.OnToggleValueChanged(value, toggle);

        Image image = toggle.GetComponent<Image>();
        if (image != null)
        {
            if (value)
            {
                this.TriggerEvent(EventName.ResetModification, toggle);
                image.color = toggleOnColor;

            }
            else
            {
                image.color = toggleOffColor;
            }
        }

        switch (toggle.name)
        {
            case "Move":
                ModifyPosition(value);
                break;
            case "Rotate":
                ModifyRotation(value);
                break;
        }
    }
    private void ResetModification(Toggle activeToggle)
    {
        foreach (Toggle toggle in toggles)
        {
            if (toggle == activeToggle)
            {
                continue;
            }
            toggle.isOn = false;
            Image image = toggle.GetComponent<Image>();
            if (image != null)
            {
                image.color = toggleOffColor;
            }
        }

        this.TriggerEvent(EventName.ModifyModelRotation, attachedModel, false);
        this.TriggerEvent(EventName.ModifyModelPosition, attachedModel, false);
    }
    private void ModifyPosition(bool isOn)
    {
        this.TriggerEvent(EventName.FinishPlaceObj);
        this.TriggerEvent(EventName.ModifyModelPosition, attachedModel, isOn);
    }
    private void ModifyRotation(bool isOn)
    {
        this.TriggerEvent(EventName.FinishPlaceObj);
        this.TriggerEvent(EventName.ModifyModelRotation, attachedModel, isOn);
    }

    private void Return()
    {
        UI3DManager.Instance.DestroySpecificPanel(this, () =>
        {
            UI3DManager.Instance.ShowPanelOnSpecificCanvas<ModeSelectionPanel>(nameof(ModeSelectionPanel), transform.parent, (panel) =>
            {
                itemInteractable.UpdateCurrentPanel(panel);
            });
        });
    }
    private void DeleteModel()
    {
        this.TriggerEvent(EventName.DeleteAnchorWithObj, attachedModel);
    }
}
