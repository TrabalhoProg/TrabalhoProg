using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMover : MonoBehaviour
{
    public GameController gameController;

    Rigidbody2D rb;
    public Vector2 forcaRef;
    public float forca, angulo, timeToDestroy = 4;
    public bool isRight = false;
    public int whoShot;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();
        Vector3 dir = isRight ? Quaternion.AngleAxis(angulo, Vector3.forward) * Vector3.right : Quaternion.AngleAxis(angulo, Vector3.back) * Vector3.left;
        rb.AddForce(dir * forca);
        StartCoroutine(DestroyByTIme());
    }

    IEnumerator DestroyByTIme()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (whoShot == 1)
        {
            if (collision.gameObject.CompareTag("Player2"))
            {
                Destroy(gameObject);
                gameController.RemoveEscudo(2);
            }
        }

        else
        {
            if (collision.gameObject.CompareTag("Player1"))
            {
                Destroy(gameObject);
                gameController.RemoveEscudo(1);
            }
        }

    }
}
