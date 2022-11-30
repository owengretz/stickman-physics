using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAnimator : MonoBehaviour
{
    public StickmanController controller;
    public HandControl hand;

    public float rotateSpeed;

    [Header("Body Parts")]
    public Rigidbody2D legLeftUpper;
    public Rigidbody2D legLeftLower;
    public Rigidbody2D legRightUpper;
    public Rigidbody2D legRightLower;

    [Header("Jump")]
    public float legLeftUpperJumpRot;
    public float legLeftLowerJumpRot;
    public float legRightUpperJumpRot;
    public float legRightLowerJumpRot;

    private void FixedUpdate()
    {
        if (!controller.grounded && !hand.grabbing)
        {
            legLeftUpper.transform.rotation = Quaternion.Slerp(legLeftUpper.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legLeftUpperJumpRot)), Time.fixedDeltaTime * rotateSpeed);
            legLeftLower.transform.rotation = Quaternion.Slerp(legLeftLower.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legLeftLowerJumpRot)), Time.fixedDeltaTime * rotateSpeed * 3);
            legRightUpper.transform.rotation = Quaternion.Slerp(legRightUpper.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legRightUpperJumpRot)), Time.fixedDeltaTime * rotateSpeed);
            legRightLower.transform.rotation = Quaternion.Slerp(legRightLower.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legRightLowerJumpRot)), Time.fixedDeltaTime * rotateSpeed * 3);


            //legLeftUpper.transform.rotation = Quaternion.RotateTowards(legLeftUpper.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legLeftUpperJumpRot)), lerpAmount);
            //legLeftLower.transform.rotation = Quaternion.RotateTowards(legLeftLower.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legLeftLowerJumpRot)), lerpAmount);
            //legRightUpper.transform.rotation = Quaternion.RotateTowards(legRightUpper.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legRightUpperJumpRot)), lerpAmount);
            //legRightLower.transform.rotation = Quaternion.RotateTowards(legRightLower.transform.rotation, Quaternion.Euler(new Vector3(0, 0, legRightLowerJumpRot)), lerpAmount);
            //legLeftLower.MoveRotation(legLeftUpperJumpRot);
            //Debug.Log(legRightUpper.transform.rotation);
            //legLeftLower.MoveRotation(legLeftLowerJumpRot);
            //legRightUpper.MoveRotation(legRightUpperJumpRot);
            //legRightLower.MoveRotation(legRightLowerJumpRot);
            //legLeftLower.MoveRotation(Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legLeftUpper.transform.rotation.z, legLeftUpperJumpRot, lerpAmount))));
            //legLeftLower.MoveRotation(Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legLeftLower.transform.rotation.z, legLeftLowerJumpRot, lerpAmount))));
            //legRightUpper.MoveRotation(Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legRightUpper.transform.rotation.z, legRightUpperJumpRot, lerpAmount))));
            //legRightLower.MoveRotation(Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legRightLower.transform.rotation.z, legRightLowerJumpRot, lerpAmount))));
            //legLeftUpper.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legLeftUpper.transform.rotation.z, legLeftUpperJumpRot, lerpAmount)));
            //legLeftLower.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legLeftLower.transform.rotation.z, legLeftLowerJumpRot, lerpAmount)));
            //legRightUpper.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legRightUpper.transform.rotation.z, legRightUpperJumpRot, lerpAmount)));
            //legRightLower.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legRightLower.transform.rotation.z, legRightLowerJumpRot, lerpAmount)));
            //legLeftUpper.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legLeftUpper.localRotation.z, legLeftUpperJumpRot, lerpAmount)));
            //legLeftLower.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legLeftLower.localRotation.z, legLeftLowerJumpRot, lerpAmount)));
            //legRightUpper.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legRightUpper.localRotation.z, legRightUpperJumpRot, lerpAmount)));
            //legRightLower.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legRightLower.localRotation.z, legRightLowerJumpRot, lerpAmount)));
            //Debug.Log(Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(legLeftUpper.rotation.z, legLeftUpperJumpRot, 1f))));
        }
    }

}
