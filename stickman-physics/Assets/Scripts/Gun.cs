using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public StickmanController controller;
    public HandControl handControl;

    public GameObject projectile;
    public float projectileForce;
    public float projectileSpeed;
    public float knockback;
    public float reloadTime;

    public Transform gun;
    public SpriteRenderer gunRend;
    public Rigidbody2D hand;
    public Rigidbody2D torso;

    private bool canShoot = true;

    private string clickName;
    private string useWeaponButtonName;

    private void Start()
    {
        clickName = controller.leftClickName;
        useWeaponButtonName = controller.useWeaponButtonName;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (controller.dummy || controller.ragdoll)
            return;

        if (Mathf.Abs(gun.transform.rotation.z) * Mathf.Rad2Deg > 40f)
        {
            gunRend.flipY = true;
        }
        else
        {
            gunRend.flipY = false;
        }

        if (/*Input.GetAxis(clickName) > 0.1f && */Input.GetButtonDown(useWeaponButtonName) && canShoot/* || controller.playerNumber > 0 && handControl.controllerInput != Vector2.zero && Input.GetButtonDown(useWeaponButtonName) && canShoot*/)
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        canShoot = false;

        Vector2 direction = -hand.transform.right;
        GameObject bullet = Instantiate(projectile);
        bullet.transform.position = new Vector2(hand.transform.position.x, hand.transform.position.y) + direction * 2.5f;
        bullet.GetComponent<Rigidbody2D>().mass = projectileForce / 2000f;

        Vector2 difference = direction - Vector2.zero;
        float Angle = Mathf.Atan2(difference.y, difference.x);
        float AngleInDegrees = Angle * Mathf.Rad2Deg;

        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, AngleInDegrees));
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * projectileForce * projectileSpeed);
        hand.AddForce(-direction * knockback);

        Invoke("Reload", reloadTime);
    }

    private void Reload()
    {
        canShoot = true;
    }
}
