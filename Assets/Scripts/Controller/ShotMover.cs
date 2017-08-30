using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMover : MonoBehaviour
{

    Rigidbody2D rb;
    public Vector2 forcaRef;
    public float forca,angulo,timeToDestroy=4;
    public bool isRight=false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 dir = isRight?Quaternion.AngleAxis(angulo, Vector3.forward) * Vector3.right: Quaternion.AngleAxis(angulo, Vector3.back) * Vector3.left;
        rb.AddForce(dir * forca);
        StartCoroutine(DestroyByTIme());
    }

    IEnumerator DestroyByTIme()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
