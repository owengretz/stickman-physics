using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public StickmanController controller;
    public HandControl hand;
    public float pointForce;
    public float swingForce;
    public float swingDuration;
    public float reloadTime;
    public float killThreshold;

    private float velocity = 0;
    private Vector3 lastPosition = Vector3.zero;

    private bool swinging;
    private bool canSwing = true;

    private string clickName;
    private string useWeaponButtonName;

    public GameObject myHead;

    private void Start()
    {
        hand.pointForce = pointForce;

        clickName = hand.buttonPressedName;
        useWeaponButtonName = controller.useWeaponButtonName;

        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        velocity = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == myHead)
            return;

        if (collision.gameObject.name == "head" && velocity > killThreshold || collision.gameObject.name == "head" && swinging)
        {
            collision.gameObject.GetComponentInParent<StickmanController>().Ragdoll();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "head" && velocity > killThreshold || collision.gameObject.name == "head" && swinging)
    //    {
    //        collision.gameObject.GetComponentInParent<StickmanController>().Ragdoll();
    //    }
    //}

    private void Update()
    {
        if (/*Input.GetAxis(hand.buttonPressedName) > 0.1f && */Input.GetButtonDown(useWeaponButtonName) && canSwing)
        {
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing()
    {
        swinging = true;
        canSwing = false;

        Vector2 dir;
        if (Mathf.Abs(transform.rotation.z) * Mathf.Rad2Deg > 40f)
        {
            dir = hand.hand.transform.up;
        }
        else
        {
            dir = -hand.hand.transform.up;
        }


        float timer = swingDuration;
        while (timer > 0f)
        {
            hand.hand.AddForce(dir * swingForce);
            hand.torso.AddForce(-dir * swingForce);

            timer -= Time.deltaTime;
            yield return null;
        }

        Invoke("Reload", reloadTime);
    }

    private void Reload()
    {
        swinging = false;
        canSwing = true;
    }

    private void OnEnable()
    {
        controller.GetComponent<DontCollideWithSelf>().IgnoreCollision();
    }
}
