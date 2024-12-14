using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// </summary>
public class BaseUIPanel : BasePanel
{
    /// <summary>
    /// </summary>
    private Dictionary<string, List<UIBehaviour>> uiDic = new Dictionary<string, List<UIBehaviour>>();
    protected virtual void Awake()
    {
        AddChildrenUIComponentsToDic<Button>();
        AddChildrenUIComponentsToDic<TextMeshProUGUI>();
        AddChildrenUIComponentsToDic<Image>();
        AddChildrenUIComponentsToDic<Toggle>();
        AddChildrenUIComponentsToDic<Slider>();
        AddChildrenUIComponentsToDic<ScrollRect>();
        AddChildrenUIComponentsToDic<TMP_InputField>();
    }                                                                                                                                                        
    protected virtual void OnDestroy()
    {
        ClearDic();
    }
    protected virtual void Update() { }
    
    /// <summary>
    /// </summary>
    /// <param name="buttonObjName"></param>
    /// <param name="button"></param>
    protected virtual void OnClick(Button button)
    {
        
    }
    /// <summary>
    /// </summary>
    /// <param name="toggleObjName"></param>
    /// <param name="value">Toggle</param>
    /// <param name="toggle"></param>
    protected virtual void OnToggleValueChanged(bool value, Toggle toggle) { }
    /// <summary>
    /// </summary>
    /// <param name="value">Slider</param>
    /// <param name="slider"></param>
    protected virtual void OnSliderValueChanged(float value,Slider slider) { }
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void AddChildrenUIComponentsToDic<T>() where T: UIBehaviour
    {        
        if (uiDic is null)
            return;

        var components = GetComponentsInChildren<T>(true); 
        foreach (var uiComponent in components)
        {
            var objName = uiComponent.gameObject.name;
            if (!uiDic.ContainsKey(objName))
                uiDic.Add(objName, new List<UIBehaviour> { uiComponent });
            else
                uiDic[objName].Add(uiComponent);

            switch (uiComponent)
            {
                
                case Button buttonComponent:
                    buttonComponent.onClick.AddListener(() =>
                    {
                        OnClick(buttonComponent);                    
                    });
                    break;
                
                case Toggle toggleComponent:
                    toggleComponent.onValueChanged.AddListener((value) =>
                    {
                        OnToggleValueChanged(value, toggleComponent);                       
                    });
                    break;
                case Slider sliderComponent:
                    sliderComponent.onValueChanged.AddListener((value) =>
                    {
                        OnSliderValueChanged(value, sliderComponent);
                    });
                    break;
            }
        }
    }
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetUIComponentByName<T>(string name) where T: UIBehaviour
    {
        if (!uiDic.ContainsKey(name))
        {
            return null;
        }
            

        foreach (var uiBehaviour in uiDic[name])
        {
            if (uiBehaviour is T castBehaviour)
                return castBehaviour;
        }
        return null;
    }

    public void ClearDic()
    {
        uiDic?.Clear();
        UI3DManager.Instance.RemovePanelFromPanelDic(name); 
    }
    protected void DestroySelf(Action onFinish = null, Action onBegin = null)
    {
        UI3DManager.Instance.DestroyPanel(gameObject.name, () =>
        {
            onFinish?.Invoke();
        }, onBegin);
    }
    protected void HideSelf(Action onFinish = null, Action onBegin = null, bool needSavePanel = false)
    {
        UI3DManager.Instance.HidePanel(gameObject.name, () =>
        {
            onFinish?.Invoke();
        }, onBegin, needSavePanel);
    }
}
