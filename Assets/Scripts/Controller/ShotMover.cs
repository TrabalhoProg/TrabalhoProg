using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

public class ShotMover : MonoBehaviour
{
    GameController gameController;
    public GameObject Explosion;

    int dano = 1;

    public int Dano { get { return dano; } }

    Rigidbody2D rb;
    Animator shotAnimator;
    public Vector2 forcaRef;
    public float forca, angulo, timeToDestroy = 10f;
    public bool isRight = false;
    public int whoShot;

    SpriteRenderer shotImage;
    public bool canChangeColor;
    public Color[] colorsToChange;
    public float timeToChange;
    float nextColorChange;
    int colorIndex = 0;

    public bool shotDisperse;
    public float timeToDisperse;

    public bool removeMovimento;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();
        shotAnimator = GetComponent<Animator>();
        shotImage = GetComponent<SpriteRenderer>();
        Vector3 dir = isRight ? Quaternion.AngleAxis(angulo, Vector3.forward) * Vector3.right : Quaternion.AngleAxis(angulo, Vector3.back) * Vector3.left;
        rb.AddForce(dir * forca);
        if (shotDisperse)
        {
            StartCoroutine(Disperse());

        }
        dano = 1;
        StartCoroutine(DestroyByTIme());
    }

    IEnumerator DestroyByTIme()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
    IEnumerator Disperse()
    {
        yield return new WaitForSeconds(timeToDisperse);
        Instantiate(Explosion, transform.position, transform.rotation);
        var temp = gameObject;
        temp.GetComponent<ShotMover>().shotDisperse = false;
        temp.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        temp.GetComponent<ShotMover>().forca = Mathf.Abs(50f * rb.velocity.x);
        Vector3 dir = isRight ? Quaternion.AngleAxis(angulo, Vector3.forward) * Vector3.right : Quaternion.AngleAxis(angulo, Vector3.back) * Vector3.left;
        rb.AddForce(dir * forca);
        temp.GetComponent<ShotMover>().angulo = 20f;
        Instantiate(temp, transform.position, transform.rotation);
        temp.GetComponent<ShotMover>().angulo = -20f;
        Instantiate(temp, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void Update()
    {
        if (shotAnimator != null)
        {
            shotAnimator.SetFloat("ySpeed", rb.velocity.y);
            shotAnimator.SetFloat("xSpeed", rb.velocity.x);
        }
        if (canChangeColor && Time.time > nextColorChange)
        {
            shotImage.color = colorsToChange[colorIndex];
            nextColorChange = Time.time + timeToChange;
            colorIndex++;
            if (colorIndex >= colorsToChange.Length)
            {
                colorIndex = 0;
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameController.IsDoubleDano())
        {
            dano++;
        }
        if (whoShot == 1)
        {
            if (collision.gameObject.CompareTag("Player2"))
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                for (int i = 0; i < dano; i++)
                {
                    if (gameController.RemoveEscudo(2))
                    {
                        collision.gameObject.GetComponent<View.PlayerView>().Morrer();
                    }
                }
                if (removeMovimento)
                {
                    collision.gameObject.GetComponent<PlayerController>().RemoveMovimento();
                }
            }
        }

        else
        {
            if (collision.gameObject.CompareTag("Player1"))
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                for (int i = 0; i < dano; i++)
                {
                    if (gameController.RemoveEscudo(1))
                    {
                        collision.gameObject.GetComponent<View.PlayerView>().Morrer();
                    }
                }
                if (removeMovimento)
                {
                    collision.gameObject.GetComponent<PlayerController>().RemoveMovimento();
                }
            }
        }

    }
}
