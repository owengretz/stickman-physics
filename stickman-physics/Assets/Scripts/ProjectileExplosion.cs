using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    //public float explosionForce;
    //public float explosionRadius;
    //public float upliftModifier;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        //if (rb != null)
        //{
        //    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        //}


        //if (collision.gameObject.name == "head")
        //{

        //}


        //if (rb != null)
        //{
        //    if (rb.transform.parent != null)
        //    {
        //        if (rb.transform.parent.GetComponent<StickmanController>() != null)
        //        {
        //            Debug.Log("hello?");

        //            foreach (_Muscle bodyPart in rb.transform.parent.GetComponent<StickmanController>().muscles)
        //            {
        //                bodyPart.bone.AddExplosionForce(explosionForce, transform.position, 10f);
        //            }
        //        }
        //    }
        //}

        //List<Rigidbody2D> rbs = new List<Rigidbody2D>();

        //Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius*5f);
        //foreach (Collider2D col in cols)
        //{
        //    Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        //    if (rb != null)
        //    {
        //        if (rb.isKinematic == false/* && rb.gameObject.layer !=8*/)
        //        {
        //            rbs.Add(rb);
        //        }
        //    }
        //}

        //foreach (Rigidbody2D rb in rbs)
        //{
        //    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

        //    //Vector2 dir = (rb.transform.position - transform.position);
        //    //float wearoff = 1 - (dir.magnitude / explosionRadius);
        //    ////if (wearoff > 0.4f)
        //    ////    wearoff = 1f;
        //    //if (wearoff > 0f)
        //    //{
        //    //    Vector2 force = dir.normalized * explosionForce * wearoff;
        //    //    //if (rb.mass < 1f)
        //    //    //{
        //    //    //    force *= rb.mass;
        //    //    //}
        //    //    Debug.Log(force);
        //    //    rb.AddForce(force, ForceMode2D.Impulse);
        //    //    //if (rb.gameObject.layer != 8)
        //    //    //{
        //    //    //    rb.AddForce(force, ForceMode2D.Impulse);
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    rb.AddForce(force, ForceMode2D.Impulse/* / 10f*/);
        //    //    //}
        //    //}
        //}

        //if (collision.gameObject.layer == 8)
        //{
        //    Vector2 dir = (collision.rigidbody.transform.position - transform.position);
        //    float wearoff = 1 - (dir.magnitude / explosionRadius);
        //    if (wearoff > 0f)
        //    {
        //        collision.rigidbody.AddForce(dir.normalized * explosionForce * wearoff);
        //    }

        //    if (collision.gameObject.name == "head")
        //    {
        //        collision.gameObject.GetComponentInParent<StickmanController>().Ragdoll();
        //    }
        //}

        if (collision.gameObject.name == "head")
        {
            collision.gameObject.GetComponentInParent<StickmanController>().Ragdoll();
        }

        StartCoroutine(DestroyBullet());
    }


    private IEnumerator DestroyBullet()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}
