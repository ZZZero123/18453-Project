using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionPanel : ModelAttachedUIPanel
{
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "TransformEditMode":
                OnClickTransformEdit();
                break;
            case "ColorChangeMode":
                OnClickColorChange();
                break;
        }
    }
    private void OnClickTransformEdit()
    {
        UI3DManager.Instance.DestroySpecificPanel(this, () =>
        {
            UI3DManager.Instance.ShowPanelOnSpecificCanvas<TransformEditPanel>(nameof(TransformEditPanel), transform.parent, (panel) =>
            {
                itemInteractable.UpdateCurrentPanel(panel);
            });
        });

    }
    private void OnClickColorChange()
    {
        UI3DManager.Instance.DestroySpecificPanel(this, () =>
        {
            UI3DManager.Instance.ShowPanelOnSpecificCanvas<ColorChangePanel>(nameof(ColorChangePanel), transform.parent, (panel) =>
            {
                itemInteractable.UpdateCurrentPanel(panel);
            });
        });
    }
}
