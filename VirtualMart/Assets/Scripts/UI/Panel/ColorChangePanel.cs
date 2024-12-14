using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangePanel : ModelAttachedUIPanel
{
    public Transform colorUIParent; 
    public GameObject colorCell;

    private Dictionary<Button, Material> buttonMaterialDic = new Dictionary<Button, Material>();

    private MeshRenderer[] renderersToChangeColor;

    private ColorChoice colorChoice;
    protected override void Start()
    {
        base.Start();
        colorChoice = attachedModel.GetComponentInChildren<ColorChoice>();
        renderersToChangeColor = itemInteractable.renderersToChangeColor;
        StartCoroutine(InitColorCells());
    }
    private IEnumerator InitColorCells()
    {
        for (int i = 0; i < colorChoice.materialElements.Count; i++)
        {
            yield return StartCoroutine(InitColorCell());
        }
        InitColorButtonMapper(); 
    }
    private IEnumerator InitColorCell()
    {
        yield return null;
        GameObject buttonUI = GameObject.Instantiate(colorCell); 
        buttonUI.transform.SetParent(colorUIParent, false); 
        Button button = buttonUI.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                ChangeMaterial(button);
            });
        }
    }
    private void InitColorButtonMapper()
    {
        for (int i = 0; i < colorUIParent.childCount; i++)
        {
            Transform colorUI = colorUIParent.GetChild(i);
            Image colorImage = colorUI.GetComponent<Image>();
            if (colorImage != null)
            {
                Sprite sprite = colorChoice.materialElements[i].sprite;
                colorImage.sprite = sprite;
                Button button = colorUI.GetComponent<Button>();
                if (button != null)
                {
                    buttonMaterialDic[button] = colorChoice.materialElements[i].material;
                }
            }
        }
    }
    private void ChangeMaterial(Button button)
    {
        Material material = buttonMaterialDic[button];
        if (material != null && renderersToChangeColor != null && renderersToChangeColor.Length > 0)
        {
            for (int i = 0; i < renderersToChangeColor.Length; i++)
            {
                renderersToChangeColor[i].material = material;
            }
            for (int i = 0; i < colorChoice.materialElements.Count; i++)
            {
                if (material == colorChoice.materialElements[i].material)
                {
                    itemInteractable.materialIndex = i;
                    break;
                }
            }
        }
    }
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Return":
                Return();
                break;
            case "Reset":
                itemInteractable.ResetColor();
                break;
        }
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
}
