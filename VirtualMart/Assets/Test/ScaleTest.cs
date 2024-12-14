using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTest : MonoBehaviour
{
    public GameObject testModel;
    public SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Vector3 targetScale = ScaleUtil.ScaleToFit(testModel, sphereCollider.bounds);
            testModel.transform.localScale = new Vector3(testModel.transform.localScale.x * targetScale.x, 
                testModel.transform.localScale.y * targetScale.y, testModel.transform.localScale.z * targetScale.z);
            testModel.transform.position = sphereCollider.transform.position;
        }
    }
}
