using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResLoadTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResourceManager.Instance.LoadAsync<GameObject>("A", (obj) =>
            {
                obj.transform.position = new Vector3(0, 1, 0);
                obj.transform.rotation = Quaternion.identity;
            });
        }
    }
    public void OnObjLoaded(GameObject obj)
    {
        obj.transform.position = new Vector3(0, 1, 0);
        obj.transform.rotation = Quaternion.identity;
    }
}
