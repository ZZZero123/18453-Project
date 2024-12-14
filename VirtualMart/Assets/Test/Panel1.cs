using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel1 : BaseFadePanel
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetUIComponentByName<Button>("Button");
        print(button.name);
    }
    
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        switch (button.name)
        {
            case "Button":
                OnButtonClick();
                break;
            case "Button2":

                break;
        }
    }
    private void OnButtonClick()
    {
        UI3DManager.Instance.HidePanel("Panel1", OnPanelHideFinish, OnPanelHideBegin);
    }
    private void OnPanelHideFinish()
    {
        UI3DManager.Instance.ShowPanel<Panel2>(nameof(Panel2), CanvasName.Canvas, (panel) =>
        {
            print(panel.name);
        });
    }
    private void OnPanelHideBegin()
    {
        print("¿ªÊ¼Òþ²Ø");
    }
}
