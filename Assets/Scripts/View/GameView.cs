using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{

    public GameController gameController;

    public GameObject player1, player2;

    public Transform player1ArmaTransform, player2ArmaTransform, player1ShotReference, player2ShotReference;

    public Text movementText, anguloText;
    public Slider forcaSlider;


    private void Start()
    {
        Transform[] temp = player1.GetComponentsInChildren<Transform>();
        foreach (var item in temp)
        {
            if (item.CompareTag("Arma"))
            {
                player1ArmaTransform = item;
            }
            else if (item.CompareTag("ShotReference"))
            {
                player1ShotReference = item;
            }
        }
        temp = player2.GetComponentsInChildren<Transform>();
        foreach (var item in temp)
        {
            if (item.CompareTag("Arma"))
            {
                player2ArmaTransform = item;
            }
            else if (item.CompareTag("ShotReference"))
            {
                player2ShotReference = item;
            }
        }
        forcaSlider.maxValue = gameController.playerMaxForce;
        UpdateUI();
    }

    private void Update()
    {
        if (gameController.hasControl)
        {
            Movimentar(gameController.GetPlayerSide() == 1 ? player1 : player2, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            AjustarAngulo(gameController.GetPlayerSide() == 1 ? player1ArmaTransform : player2ArmaTransform, Input.GetAxis("AnguloArma"));

            CarregarTiro();
        }
    }

    private void LateUpdate()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        anguloText.text = gameController.GetCurrentAngulo().ToString();
        movementText.text = gameController.PlayerMovement.ToString();
        forcaSlider.value = gameController.CurrentPlayerForce;
    }

    public void Movimentar(GameObject player, float xMovement, float yMovement)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if ((xMovement != 0 || yMovement != 0) && !gameController.isShotCharging)
        {
            if (gameController.PlayerMovement > 0)
            {
                float x = xMovement * gameController.playerSpeed * Time.deltaTime;
                float y = yMovement * gameController.playerSpeed * Time.deltaTime;
                playerRb.velocity = new Vector2(x, y);
                if (gameController.PlayerMovement > 0)
                    gameController.PlayerMovement = gameController.PlayerMovement - (Mathf.Abs(x) + Mathf.Abs(y));
            }
            else
            {
                playerRb.velocity = Vector2.zero;
                gameController.PlayerMovement = 0;
            }
        }
        else
        {
            playerRb.velocity = Vector2.zero;
        }

    }

    public void CarregarTiro()
    {
        if (!gameController.isShotCharging && Input.GetButton("Fire1"))
        {
            gameController.isShotCharging = true;
        }
        if (gameController.isShotCharging && Input.GetButton("Fire1"))
        {
            gameController.CurrentPlayerForce += gameController.shotForce;
        }
        else if (gameController.isShotCharging && !Input.GetButton("Fire1"))
        {
            gameController.isShotCharging = false;
            Atirar();
            gameController.CurrentPlayerForce = 0;
            IniciarNovoTurno();
        }
    }

    public void Atirar()
    {
        GameObject shot = gameController.Atirar();
        shot.GetComponent<ShotMover>().forca = gameController.CurrentPlayerForce;
        shot.GetComponent<ShotMover>().angulo = gameController.GetCurrentAngulo();
        shot.GetComponent<ShotMover>().isRight = gameController.GetPlayerSide() == 1;
        shot.GetComponent<ShotMover>().whoShot = gameController.GetPlayerSide();
        if (gameController.GetPlayerSide() == 1)
        {
            Instantiate(shot, player1ShotReference);
        }
        else
        {
            Instantiate(shot, player2ShotReference);
        }
    }

    public void AjustarAngulo(Transform arma, float angulo)
    {
        if (angulo != 0)
        {
            float rotation = gameController.AjustarAngulo(angulo);
            arma.rotation = Quaternion.Euler(0f, 0f, gameController.GetPlayerSide() == 1 ? rotation : -rotation);
        }

    }

    public void IniciarNovoTurno()
    {
        gameController.ChangePlayerSide();
        gameController.isShotCharging = false;
        gameController.CurrentPlayerForce = 0f;
        gameController.PlayerMovement = (float)gameController.GetCurrentMaxMovement() * 100;
    }
}
