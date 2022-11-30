using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontCollideWithSelf : MonoBehaviour
{
    public Collider2D[] colliders;

    void OnEnable()
    {
        IgnoreCollision();
    }

    public void IgnoreCollision()
    {
        // < length -1 because the last doesn't need to ignore itself ;-)
        for (int i = 0; i < colliders.Length - 1; i++)
        {
            Collider2D firstCollider = colliders[i];
            // loop through all colliders firstCollider has to ignore
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Collider2D secondCollider = colliders[j];
                Physics2D.IgnoreCollision(firstCollider, secondCollider);
            }
        }
    }
}
