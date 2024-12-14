using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadAnchorPanel : BaseFadePanel
{
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Continue":
                Continue();
                break;
            case "Restart":
                Restart();
                break;
        }
    }
    private void Continue()
    {
        DestroySelf(() =>
        {
            this.TriggerEvent(EventName.LoadAnchors);
            Destroy(transform.parent.gameObject); 
            UI3DManager.Instance.ShowPanel<InventoryPanel>(nameof(InventoryPanel), CanvasName.ModelInventoryCanvas, (panel) =>
            {
                Transform canvas = panel.transform.parent;
                UIMenuDelayFollowHead uiFollow = canvas.GetComponent<UIMenuDelayFollowHead>();
                uiFollow.enabled = true;
                panel.ControlRayInteractable(true);
            });
        });
    }
    private void Restart()
    {
        DestroySelf(() =>
        {
            this.TriggerEvent(EventName.DeleteAllAnchorsWithObjs);
            Destroy(transform.parent.gameObject); 
            UI3DManager.Instance.ShowPanel<InventoryPanel>(nameof(InventoryPanel), CanvasName.ModelInventoryCanvas, (panel) =>
            {
                Transform canvas = panel.transform.parent;
                UIMenuDelayFollowHead uiFollow = canvas.GetComponent<UIMenuDelayFollowHead>();
                uiFollow.enabled = true;
                panel.ControlRayInteractable(true);
            });
        });
    }

}
