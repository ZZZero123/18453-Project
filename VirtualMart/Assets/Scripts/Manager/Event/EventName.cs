using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventName 
{
    TestChangeMaterial,
    ModifyModelRotation,
    ModifyModelPosition,
    ResetModification,
    FinishPlaceObj,
    StartPlaceObj,
    CreateAndSaveAnchor,
    DeleteAnchor,
    DeleteAnchorWithObj,
    LoadAnchors,
    DeleteAllAnchorsWithObjs,
    OnAnchorCreateSuccess,
    OnAnchorDeleteSuccess,
}
