using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private int clickEnableInventoryCount;
    // Start is called before the first frame update
    void Awake()
    {
        UI3DManager.Instance.InitCanvas();
        CheckIfAnchorsExist();
    }

    private void CheckIfAnchorsExist()
    {

        List<AnchorData> allAnchors = JsonManager.Instance.LoadData<List<AnchorData>>("Anchors");
        if (allAnchors.Count > 0)
        {
            GameObject modelInventoryCanvas = UI3DManager.Instance.GetCanvasByName(CanvasName.ModelInventoryCanvas);
            UIMenuDelayFollowHead uiFollow = modelInventoryCanvas.GetComponent<UIMenuDelayFollowHead>();
            uiFollow.enabled = false;
            RayInteractable rayInteractable = modelInventoryCanvas.GetComponentInChildren<RayInteractable>();
            rayInteractable.enabled = false;
            UI3DManager.Instance.ShowPanel<LoadAnchorPanel>(nameof(LoadAnchorPanel), CanvasName.ContinueGameCanvas);
        }
        else
        {
            GameObject continueGameCanvas = UI3DManager.Instance.GetCanvasByName(CanvasName.ContinueGameCanvas);
            Destroy(continueGameCanvas.gameObject);
            UI3DManager.Instance.ShowPanel<InventoryPanel>(nameof(InventoryPanel), CanvasName.ModelInventoryCanvas, (panel) =>
            {
                panel.ControlRayInteractable(true);
            });
        }
    }
    public void ShowInventory()
    {
        InventoryPanel panel = UI3DManager.Instance.GetPanel<InventoryPanel>(nameof(InventoryPanel));
        if (panel == null)
        {
            return;
        }
        if (clickEnableInventoryCount % 2 == 0)
        {
            panel.ControlRayInteractable(false);
            UI3DManager.Instance.HidePanel(nameof(InventoryPanel));
        }
        else
        {
            UI3DManager.Instance.ShowPanel<InventoryPanel>(nameof(InventoryPanel), CanvasName.ModelInventoryCanvas, (panel) =>
            {
                panel.ControlRayInteractable(true);
            });
        }
        clickEnableInventoryCount = (clickEnableInventoryCount + 1) % 2; // (0+1)%2=1, (1+1)%2=0
    }
}
