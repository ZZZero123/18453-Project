using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChoice : MonoBehaviour
{
    [Serializable]
    public class MaterialElement
    {
        public Material material;
        public Sprite sprite;
    }
    public List<MaterialElement> materialElements = new List<MaterialElement>();
}
