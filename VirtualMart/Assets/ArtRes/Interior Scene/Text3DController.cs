using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text3DController : MonoBehaviour
{
    private GameObject _mainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        _mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        //物体始终面向摄像机
        var rotation = Quaternion.LookRotation(_mainCamera.transform.TransformVector(Vector3.forward),
            _mainCamera.transform.TransformVector(Vector3.up));
        rotation = new Quaternion(0, rotation.y, 0, rotation.w);
        gameObject.transform.rotation = rotation;
    }


}
