using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

public class ShotMover : MonoBehaviour
{
    GameController gameController;
    public GameObject Explosion;

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
                Instantiate(Explosion,transform.position,transform.rotation);
                Destroy(gameObject);
                if (gameController.RemoveEscudo(2))
                {
                    collision.gameObject.GetComponent<View.PlayerView>().Morrer();
                }
            }
        }

        else
        {
            if (collision.gameObject.CompareTag("Player1"))
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                if (gameController.RemoveEscudo(1))
                {
                    collision.gameObject.GetComponent<View.PlayerView>().Morrer();
                }
            }
        }

    }
}
