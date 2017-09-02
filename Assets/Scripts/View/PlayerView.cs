using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Controller;

namespace View
{
    public class PlayerView : MonoBehaviour
    {
        GameController gameController;
        PlayerController playerController;

        public GameObject Explosion;

        public RectTransform EscudoHolder, MunicaoHolder;
        public GameObject EscudoUI, ShotUI;
        int currentEscudo, escudoPadding = 0, municaoPadding = 0;
        Stack<GameObject> escudosObj;

        public Transform playerArmaTransform, playerShotReference;

        Rigidbody2D playerRb;

        public int thisPlayer;
        float forceMultiplier = 1;

        Vector3 lastLocationRef;

        void Start()
        {
            playerRb = GetComponent<Rigidbody2D>();

            lastLocationRef = transform.position;

            Transform[] temp = GetComponentsInChildren<Transform>();
            foreach (var item in temp)
            {
                if (item.CompareTag("Arma"))
                {
                    playerArmaTransform = item;
                }
                else if (item.CompareTag("ShotReference"))
                {
                    playerShotReference = item;
                }
            }
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

            playerController = GetComponent<PlayerController>();

            escudosObj = new Stack<GameObject>();

            SetPlayerUI();
            SetMunicaoUI();
        }

        void Update()
        {
            if (gameController.hasControl && gameController.GetPlayerSide() == thisPlayer)
            {
                Movimentar(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                AjustarAngulo(Input.GetAxis("AnguloArma"));

                CarregarTiro();
            }
            else
            {
                playerRb.velocity = Vector2.zero;
            }
        }

        private void LateUpdate()
        {
            SetPlayerUI();
            AjustarRotacaoArma();

            if (gameController.GetPlayerSide() == thisPlayer)
            {
                //MunicaoHolder.gameObject.SetActive(true);
            }
            else
            {
                //MunicaoHolder.gameObject.SetActive(false);
            }

            if (gameController.GetGameState() == 2)
            {
                EscudoHolder.gameObject.SetActive(true);
                MunicaoHolder.gameObject.SetActive(true);
            }
            else
            {
                EscudoHolder.gameObject.SetActive(false);
                MunicaoHolder.gameObject.SetActive(false);
            }

        }

        public void SetPlayerUI()
        {

            if (currentEscudo != playerController.GetCurrentEscudo())
            {
                currentEscudo = playerController.GetCurrentEscudo();
                foreach (var item in escudosObj)
                {
                    Destroy(item.gameObject);
                    escudoPadding = 0;
                }
                escudosObj = new Stack<GameObject>();
                for (int i = 0; i < currentEscudo; i++)
                {
                    var tempObj = Instantiate(EscudoUI, EscudoHolder);
                    escudosObj.Push(tempObj);
                    RectTransform tempTransform = tempObj.GetComponent<RectTransform>();
                    if (thisPlayer == 2)
                    {
                        tempTransform.anchorMax = new Vector2(1, 1);
                        tempTransform.anchorMin = new Vector2(1, 1);
                        tempTransform.pivot = new Vector2(1, 1);
                    }
                    tempTransform.position = new Vector3(tempTransform.position.x, tempTransform.position.y - escudoPadding, tempTransform.position.z);
                    escudoPadding++;
                }
            }
        }

        public void SetMunicaoUI()
        {
            foreach (var item in MunicaoHolder.GetComponentsInChildren<Image>())
            {
                Destroy(item.gameObject);
            }
            municaoPadding = 0;
            var tempQueue = playerController.GetTiroFila();
            foreach (var item in tempQueue)
            {
                var tempObj = Instantiate(ShotUI, MunicaoHolder);
                var tempImage = item.GetComponent<SpriteRenderer>().color;
                var tempImage2 = tempObj.GetComponent<Image>().color;
                tempImage2 = tempImage;
                var tempTransform = tempObj.GetComponent<RectTransform>();
                if (thisPlayer == 2)
                {
                    tempTransform.anchorMax = new Vector2(1, 1);
                    tempTransform.anchorMin = new Vector2(1, 1);
                    tempTransform.pivot = new Vector2(1, 1);
                }
                tempTransform.position = new Vector3(tempTransform.position.x, tempTransform.position.y - municaoPadding, tempTransform.position.z);
                municaoPadding++;
            }
        }

        public void Movimentar(float xMovement, float yMovement)
        {
            if ((xMovement != 0 || yMovement != 0) && !gameController.isShotCharging)
            {
                if (gameController.PlayerMovement > 0)
                {
                    float x = xMovement * gameController.playerSpeed * Time.deltaTime;
                    float y = yMovement * gameController.playerSpeed * Time.deltaTime;
                    playerRb.velocity = new Vector2(x, y);
                    Vector3 clampedPosition = transform.position;
                    clampedPosition.y = Mathf.Clamp(transform.position.y, -8f, 8f);
                    clampedPosition.x = gameController.GetPlayerSide() == 1 ? Mathf.Clamp(transform.position.x, -16f, 0f) : Mathf.Clamp(transform.position.x, 0f, 16f);
                    transform.position = clampedPosition;
                    if (gameController.PlayerMovement > 0 && lastLocationRef != transform.position)
                    {
                        gameController.PlayerMovement = gameController.PlayerMovement - (Mathf.Abs(x) + Mathf.Abs(y));
                        lastLocationRef = transform.position;
                    }

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
                gameController.CurrentPlayerForce += gameController.shotForce * forceMultiplier;
                forceMultiplier += 100f * Time.deltaTime;
            }
            else if (gameController.isShotCharging && !Input.GetButton("Fire1"))
            {
                gameController.isShotCharging = false;
                Atirar();
                forceMultiplier = 1f;
                gameController.CurrentPlayerForce = 0;
                gameController.hasControl = false;
            }
        }

        public void Atirar()
        {
            GameObject shot = gameController.Atirar();
            SetMunicaoUI();
            shot.GetComponent<ShotMover>().forca = gameController.CurrentPlayerForce;
            shot.GetComponent<ShotMover>().angulo = playerController.GetAngulo();
            shot.GetComponent<ShotMover>().isRight = gameController.GetPlayerSide() == 1;
            shot.GetComponent<ShotMover>().whoShot = gameController.GetPlayerSide();

            Instantiate(shot, playerShotReference.transform.position, playerShotReference.transform.rotation);
            StartCoroutine(EndTurn());
        }


        IEnumerator EndTurn()
        {
            yield return new WaitForSeconds(4);
            gameController.IniciarNovoTurno();
        }

        public void AjustarAngulo(float angulo)
        {
            if (angulo != 0)
            {
                float rotation = playerController.AjustarAngulo(angulo);
            }

        }
        public void AjustarRotacaoArma()
        {
            float rotation = playerController.GetAngulo();
            playerArmaTransform.rotation = Quaternion.Euler(0f, 0f, thisPlayer == 1 ? rotation : -rotation);
        }

        public void Morrer()
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            gameController.ChangeGameState();
            gameController.AcabarPartida(gameController.GetPlayerSide());
            gameObject.SetActive(false);
            AjustarRotacaoArma();
        }

    }
}

