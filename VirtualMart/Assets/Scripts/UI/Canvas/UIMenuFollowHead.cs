using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuFollowHead : MonoBehaviour
{
    private Transform playerHead;
    // Start is called before the first frame update
    void Start()
    {
        playerHead = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowHeadRotation();
    }
    private void FollowHeadRotation()
    {
        Vector3 playerPosition = new Vector3(playerHead.position.x, transform.position.y, playerHead.position.z);
        Vector3 directionFromPlayerToCanvas = transform.position - playerPosition;
        transform.rotation = Quaternion.LookRotation(directionFromPlayerToCanvas);
    }
}
