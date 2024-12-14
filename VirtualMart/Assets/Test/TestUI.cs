using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public List<ItemData> itemDataList = new List<ItemData> ();
    // Start is called before the first frame update
    void Start()
    {
        UI3DManager.Instance.ShowPanel<Panel1>(nameof(Panel1), CanvasName.Canvas, (panel) =>
        {
            print("Panel1º”‘ÿÕÍ±œ");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
