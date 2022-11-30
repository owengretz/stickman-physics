using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControl : MonoBehaviour
{
    public StickmanController controller;
    public Rigidbody2D torso;
    public Rigidbody2D hand;
    public string mouseButton;
    [HideInInspector] public string buttonPressedName;
    public float pointForce;


    public bool holdingWeapon;
    public Rigidbody2D[] bodyParts;
    public float normalGravity;
    public float grabbingGravity;
    [HideInInspector] public bool tryingToGrab;
    [HideInInspector] public bool grabbing;
    public HingeJoint2D grabJoint;


    private float distUntilDamp = 0.001f;
    private float decelRate = 0.05f;

    [HideInInspector] public Vector2 controllerInput;

    public GameObject gun;
    public GameObject sword;

    private void Start()
    {
        if (mouseButton == "0")
        {
            buttonPressedName = controller.leftClickName;
        }
        else
        {
            buttonPressedName = controller.rightClickName;
        }
    }


    private void FixedUpdate()
    {
        if (controller.dummy || controller.ragdoll)
            return;

        if (controller.playerNumber > 0)
            controllerInput = new Vector2(Input.GetAxis(controller.horizControllerHandAimName), Input.GetAxis(controller.vertControllerHandAimName));

        // point hand at mouse
        if (Input.GetAxis(buttonPressedName) > 0.1f || holdingWeapon)
        {
            Vector2 direction;

            if (controller.playerNumber == 0)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                direction = (mousePos - torso.position).normalized;
            }
            else
            {
                direction = controllerInput;
            }

            Vector2 target = torso.position + direction * 2f;

            float lerpFactor = Mathf.Sqrt(Vector2.Lerp(hand.position, target, pointForce).magnitude);

            hand.AddForce(direction * pointForce * lerpFactor * Time.fixedDeltaTime * 50f);
            torso.AddForce(-direction * pointForce * lerpFactor * Time.fixedDeltaTime * 50f);

            float distToTarget = (hand.position - target).magnitude;
            if (distToTarget < distUntilDamp)
            {
                hand.velocity *= Mathf.Pow(decelRate, Time.fixedDeltaTime);
            }
        }
    }


    private void Update()
    {
        if (controller.dummy || controller.ragdoll)
            return;

        if (mouseButton == "0")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StopGrab();
                holdingWeapon = true;
                gun.SetActive(true);
                sword.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StopGrab();
                holdingWeapon = true;
                gun.SetActive(false);
                sword.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                holdingWeapon = false;
                gun.SetActive(false);
                sword.SetActive(false);
                return;
            }
        }

        if (holdingWeapon)
            return;

        if (Input.GetAxis(buttonPressedName) > 0.1f)
        {
            tryingToGrab = true;
        }
        else if (grabbing)
        {
            StopGrab();
        }
    }

    private void StopGrab()
    {
        grabJoint.enabled = false;
        foreach (Rigidbody2D part in bodyParts)
        {
            part.gravityScale = normalGravity;
        }
        grabbing = false;
        tryingToGrab = false;
    }
}
