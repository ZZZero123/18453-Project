using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRObjPlacer : MonoBehaviour
{
    public Player player;
    public OVRInput.Controller controllerType;
    public GameObject spawnedPrefab;
    public float rotationAngle = 2f;
    public float moveSpeed = 1f;

    private ItemData currentItemData;
    private GameObject currentSpawnedObj;
    private Action placeObjAction;
    private Action rotateObjAction;
    private Action moveObjAction;
    private bool isCreatingObj;
    private Quaternion rotationOffset = Quaternion.identity; 

    private void Awake()
    {
        EventCenter.Instance.AddListener<GameObject, bool>(EventName.ModifyModelRotation, AddRotateAction);
        EventCenter.Instance.AddListener<GameObject, bool>(EventName.ModifyModelPosition, AddMoveAction);
        EventCenter.Instance.AddListener(EventName.FinishPlaceObj, FinishPlace);
        EventCenter.Instance.AddListener<ItemData>(EventName.StartPlaceObj, StartPlace);
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveListener<GameObject, bool>(EventName.ModifyModelRotation, AddRotateAction);
        EventCenter.Instance.RemoveListener<GameObject, bool>(EventName.ModifyModelPosition, AddMoveAction);
        EventCenter.Instance.RemoveListener(EventName.FinishPlaceObj, FinishPlace);
        EventCenter.Instance.RemoveListener<ItemData>(EventName.StartPlaceObj, StartPlace);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        placeObjAction?.Invoke();
        rotateObjAction?.Invoke();
        moveObjAction?.Invoke();
    }
    private void StartPlace(ItemData itemData)
    {
        currentItemData = itemData; 
        if (controllerType == OVRInput.Controller.RTouch)
        {
            PlaceObjByRightRaycast(true);
        }
        else if (controllerType == OVRInput.Controller.LTouch)
        {
            PlaceObjByLeftRaycast(true);
        }
    }
    private void FinishPlace()
    {
        if(currentSpawnedObj == null)
        {
            return;
        }
        if(controllerType == OVRInput.Controller.RTouch)
        {
            PlaceObjByRightRaycast(false);
        }
        else if(controllerType == OVRInput.Controller.LTouch)
        {
            PlaceObjByLeftRaycast(false);
        }
    }
    private void AddPlaceAction()
    {
        placeObjAction = () =>
        {
            if(currentItemData == null || isCreatingObj)
            {
                return;
            }
            Ray ray = GetControllerRay();
            RaycastHit hit = new RaycastHit();
            MRUKAnchor anchorHit = null;
            LabelFilter labelFilter = LabelFilter.Included(currentItemData.sceneLabels);
            MRUK.Instance.GetCurrentRoom()?.Raycast(ray, Mathf.Infinity, labelFilter, out hit, out anchorHit);

            if (anchorHit != null)
            {
                if (currentSpawnedObj == null)
                {
                    isCreatingObj = true;
                    ResourceManager.Instance.LoadAsync<GameObject>(currentItemData.itemName, (spawnObj) =>
                    {
                        currentSpawnedObj = spawnObj;

                        currentSpawnedObj.name = RemoveClone(currentSpawnedObj.name);
                        if (rotateObjAction == null)
                        {
                            AddRotateAction(currentSpawnedObj, true);
                        }
                        ItemInteractable itemInteractable = currentSpawnedObj.GetComponent<ItemInteractable>();
                        if (itemInteractable != null)
                        {
                            itemInteractable.itemType = currentItemData.itemType;
                        }
                        isCreatingObj = false;
                        Quaternion lookRotation = Quaternion.identity;
                       
                    });

                }
                if (!isCreatingObj)
                {
                    Quaternion lookRotation = Quaternion.identity;
                    if (currentItemData.itemType == ItemType.Furniture)
                    {
                        lookRotation = Quaternion.FromToRotation(Vector3.up, hit.normal); // ��Y����뵽���淨��
                    }
                    else if (currentItemData.itemType == ItemType.WallDecoration)
                    {
                        lookRotation = Quaternion.LookRotation(hit.normal);
                    }
                    currentSpawnedObj.transform.rotation = lookRotation * rotationOffset;
                    currentSpawnedObj.transform.position = hit.point;
                }
            }
        };
    }
    private void AddRotateAction(GameObject objToRotate, bool isEnable)
    {
        if (!isEnable)
        {
            rotateObjAction = null;
            return;
        }

        rotateObjAction = () =>
        {
            if (objToRotate == null)
            {
                return;
            }

            Vector2 value = Vector2.zero;
            if (controllerType == OVRInput.Controller.RTouch)
            {
                value = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            }
            else if (controllerType == OVRInput.Controller.LTouch)
            {
                value = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            }

            if (value.x == 0)
            {
                return;
            }
            ItemInteractable itemInteractable = objToRotate.GetComponent<ItemInteractable>();
            if (itemInteractable == null)
            {
                return;
            }
            float rotateAmount = (value.x > 0) ? rotationAngle : -rotationAngle;
            Vector3 rotateAxis = Vector3.zero;

            if (itemInteractable.itemType == ItemType.Furniture)
            {
                rotateAxis = Vector3.up;
            }
            else if (itemInteractable.itemType == ItemType.WallDecoration)
            {
                rotateAxis = Vector3.forward;
            }

            if (placeObjAction != null)
            {
                rotationOffset *= Quaternion.AngleAxis(rotateAmount, rotateAxis);
            }
            else
            {
                objToRotate.transform.Rotate(rotateAxis, rotateAmount, Space.Self);  // ��������ת
            }

        };
    }
    private void AddMoveAction(GameObject objToMove, bool isEnable)
    {
        if (!isEnable)
        {
            moveObjAction = null;
            return;
        }
        moveObjAction = () =>
        {
            if (objToMove == null)
            {
                return;
            }
            Vector2 value = Vector2.zero;

            if (controllerType == OVRInput.Controller.RTouch)
            {
                value = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            }
            else if (controllerType == OVRInput.Controller.LTouch)
            {
                value = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            }

            if (value == Vector2.zero)
            {
                return;
            }

            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;  
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0;
            right.Normalize();

            ItemInteractable itemInteractable = objToMove.GetComponent<ItemInteractable>();
            if (itemInteractable == null)
            {
                return;
            }
            Vector3 moveDirection = Vector3.zero;
            if (itemInteractable.itemType == ItemType.Furniture)
            {
                moveDirection = (right * value.x + forward * value.y).normalized;
            }
            else if (itemInteractable.itemType == ItemType.WallDecoration)
            {
                moveDirection = (-objToMove.transform.right * value.x + objToMove.transform.up * value.y).normalized;
            }

            objToMove.transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        };
    }
    
    public void PlaceObjByRightRaycast(bool isOn)
    {
        if(controllerType == OVRInput.Controller.LTouch)
        {
            return;
        }
        if (isOn)
        {
            ClearData();
            this.TriggerEvent(EventName.ResetModification);
            AddPlaceAction();
            player.rightControllerRay.SetActive(false);
        }
        else
        {
            if(currentSpawnedObj == null)
            {
                return;
            }
            ClearData();
            ItemInteractable interactable = currentSpawnedObj.GetComponent<ItemInteractable>();
            if(interactable != null)
            {
                interactable.ConfirmPlace();
            }
            currentSpawnedObj = null;
            currentItemData = null;
            player.rightControllerRay.SetActive(true);
            
        }
    }
    public void PlaceObjByLeftRaycast(bool isOn)
    {
        if(controllerType == OVRInput.Controller.RTouch)
        {
            return;
        }
        if (isOn)
        {
            ClearData();
            this.TriggerEvent(EventName.ResetModification);
            AddPlaceAction();
            player.leftControllerRay.SetActive(false);
        }
        else
        {
            if (currentSpawnedObj == null)
            {
                return;
            }
            ClearData();
            ItemInteractable interactable = currentSpawnedObj.GetComponent<ItemInteractable>();
            if (interactable != null)
            {
                interactable.ConfirmPlace();
            }
            currentSpawnedObj = null;
            currentItemData = null;
            player.leftControllerRay.SetActive(true);
        }
    }
    private void ClearData()
    {
        placeObjAction = null;
        rotateObjAction = null;
        moveObjAction = null;
        rotationOffset = Quaternion.identity;
    }
   
    private Ray GetControllerRay()
    {
        Vector3 rayOrigin = Vector3.zero;
        Vector3 rayDirection = Vector3.zero;
        if (controllerType == OVRInput.Controller.RTouch)
        {
            rayOrigin = player.rightControllerRayStartPoint.position;
            rayDirection = player.rightControllerRayStartPoint.forward;
        }
        else if (controllerType == OVRInput.Controller.LTouch)
        {
            rayOrigin = player.leftControllerRayStartPoint.position;
            rayDirection = player.leftControllerRayStartPoint.forward;

        }


        return new Ray(rayOrigin, rayDirection);
    }
    private string RemoveClone(string name)
    {
        if (name.EndsWith("(Clone)"))
        {
            return name.Replace("(Clone)", "").Trim();
        }

        return name;
    }
}
