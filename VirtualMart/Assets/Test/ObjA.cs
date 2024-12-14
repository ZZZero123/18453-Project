using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjA : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(ChangeBObjMaterial), 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeBObjMaterial()
    {
        this.TriggerEvent(EventName.TestChangeMaterial);
        this.TriggerEvent(EventName.TestChangeMaterial, 1);
        string value = this.TriggerEventWithReturnValue<GameObject, string>(EventName.TestChangeMaterial, gameObject);
        print(value);
    }
}
