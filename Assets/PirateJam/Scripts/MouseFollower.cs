using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script makes an object follow the mouse with optional constraints 
 */

public class MouseFollower : MonoBehaviour
{
    [Tooltip("Whether to constrain on the X")]
    [SerializeField] private bool constrainOnX;

    [Tooltip("Whether to constrain on the Y")]
    [SerializeField] private bool constrainOnY;

    [Tooltip("Whether to constrain on the Z")]
    [SerializeField] private bool constrainOnZ;

    [Tooltip("Limitation on X coordinate (not used if constrained on X")]
    [SerializeField] private Vector2 xConstraintsMinMax;

    [Tooltip("Limitation on Y coordinate (not used if constrained on Y")]
    [SerializeField] private Vector2 yConstraintsMinMax;

    [Tooltip("Limitation on Z coordinate (not used if constrained on Z")]
    [SerializeField] private Vector2 zConstraintsMinMax;

    [Tooltip("The layers this object will view as a trackable surface")]
    [SerializeField] private LayerMask layerMask;

    private bool isFollowingMouse = false;

    Vector3 objectFollowPos;

    private void Update()
    {
        if (isFollowingMouse == false) return;

        GetMousePosition();
    }

    public void GetMousePosition()
    {   
        Ray mousePosRay = Camera.main.ScreenPointToRay(Input.mousePosition);        

        if (Physics.Raycast(mousePosRay, out RaycastHit hitInfo, Mathf.Infinity, layerMask)) 
        {
            objectFollowPos = hitInfo.point;
        }

        float xPos = constrainOnX ? Mathf.Clamp(objectFollowPos.x, xConstraintsMinMax[0], xConstraintsMinMax[1]) : objectFollowPos.x;
        float yPos = constrainOnY ? Mathf.Clamp(objectFollowPos.y, yConstraintsMinMax[0], yConstraintsMinMax[1]) : objectFollowPos.y;
        float zPos = constrainOnZ ? Mathf.Clamp(objectFollowPos.z, zConstraintsMinMax[0], zConstraintsMinMax[1]) : objectFollowPos.z;

        transform.position = new Vector3(xPos, yPos, zPos);
    }

    public void SetFollow(bool shouldFollow) => isFollowingMouse = shouldFollow;   
}
