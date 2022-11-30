using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public HandControl hand;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (hand.tryingToGrab == false || hand.grabbing == true)
        {
            return;
        }

        if (!col.gameObject.CompareTag("grabbable")/* && !col.gameObject.CompareTag("1 way platform")*/)
        {
            return;
        }

        hand.grabJoint.enabled = true;
        hand.grabJoint.connectedBody = col.rigidbody;
        if (col.gameObject.layer == 9)
        {
            foreach (Rigidbody2D part in hand.bodyParts)
            {
                part.gravityScale = hand.grabbingGravity;
            }
        }
        hand.grabbing = true;
    }

}
