using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjB : MonoBehaviour
{
    public Material material;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        EventCenter.Instance.AddListener(EventName.TestChangeMaterial, ChangeMaterial);
        EventCenter.Instance.AddListener<int>(EventName.TestChangeMaterial, PrintNum);
        EventCenter.Instance.AddListenerWithReturnValue<GameObject, string>(EventName.TestChangeMaterial, GetAnotherObj);
        //EventCenter.Instance.AddListenerWithReturnValue
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveListener(EventName.TestChangeMaterial, ChangeMaterial);
        EventCenter.Instance.RemoveListener<int>(EventName.TestChangeMaterial, PrintNum);
        EventCenter.Instance.RemoveListenerWithReturnValue<GameObject, string>(EventName.TestChangeMaterial, GetAnotherObj);
    }
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeMaterial()
    {
        meshRenderer.material = material;
    }

    public void PrintNum(int num)
    {
        print(num);
    }
    public string GetAnotherObj(GameObject other)
    {
        print(other.name);
        return "传入成功";
    }
    public string TestReturn()
    {
        return "OK";
    }
}
