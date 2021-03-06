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

        public AudioClip shotCharge, shotSound, gunMoveSound;

        public RectTransform EscudoHolder, MunicaoHolder;
        public GameObject EscudoUI, ShotUI, indicatorUI;
        int currentEscudo, escudoPadding = 0;
        Stack<GameObject> escudosObj;

        public Transform playerArmaTransform, playerShotReference;

        Rigidbody2D playerRb;

        public int thisPlayer;
        float forceMultiplier = 1;

        Vector3 lastLocationRef;

        public GameObject shotParticle;



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
                indicatorUI.gameObject.SetActive(true);
                var temp = playerRb.GetComponent<BoxCollider2D>();
                temp.enabled = false;
                temp = playerArmaTransform.GetComponent<BoxCollider2D>();
                temp.enabled = false;
            }
            else
            {
                indicatorUI.gameObject.SetActive(false);
                var temp = playerRb.GetComponent<BoxCollider2D>();
                temp.enabled = true;
                temp = playerArmaTransform.GetComponent<BoxCollider2D>();
                temp.enabled = true;
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
            var tempQueue = playerController.GetTiroFila();
            var temp = MunicaoHolder.GetComponentsInChildren<Image>();
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].sprite = tempQueue.ToArray()[i].GetComponent<SpriteRenderer>().sprite;
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

                gameController.audioSrc.clip = shotCharge;
                gameController.audioSrc.volume = 0;
                gameController.audioSrc.loop = true;
                gameController.audioSrc.Play();

            }
            if (gameController.isShotCharging && Input.GetButton("Fire1"))
            {
                gameController.CurrentPlayerForce += gameController.shotForce * forceMultiplier;
                forceMultiplier += 100f * Time.deltaTime;
                if (gameController.audioSrc.volume < 1)
                {
                    gameController.audioSrc.volume += 0.005f;
                }
            }
            else if (gameController.isShotCharging && !Input.GetButton("Fire1"))
            {
                gameController.isShotCharging = false;

                gameController.audioSrc.loop = false;
                gameController.audioSrc.Stop();
                gameController.audioSrc.volume = 1f;
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
            //Instantiate(shotParticle, playerShotReference.transform.position, playerArmaTransform.transform.rotation);
            playerArmaTransform.GetComponent<Animator>().SetTrigger("Shoot");
            Instantiate(shot, playerShotReference.transform.position, playerShotReference.transform.rotation);
            gameController.PlaySound(shotSound);
            StartCoroutine(EndTurn());
        }


        IEnumerator EndTurn()
        {
            yield return new WaitForSeconds(4);
            gameController.IniciarNovoTurno();
        }

        public void AjustarAngulo(float angulo)
        {
            if (!gameController.isShotCharging)
            {

                if (angulo != 0)
                {
                    float rotation = playerController.AjustarAngulo(angulo);
                    if (!gameController.audioSrc.isPlaying && rotation < 60f && rotation > 0)
                    {
                        gameController.audioSrc.clip = gunMoveSound;
                        gameController.audioSrc.loop = true;
                        gameController.audioSrc.Play();
                    }
                    else if (rotation >= 60f || rotation <= 0)
                    {
                        gameController.audioSrc.loop = false;
                        gameController.audioSrc.Stop();
                    }
                }
                else
                {
                    gameController.audioSrc.loop = false;
                    gameController.audioSrc.Stop();
                }

            }
        }
        public void AjustarRotacaoArma()
        {
            float rotation = playerController.GetAngulo();
            playerArmaTransform.rotation = Quaternion.Euler(0f, 0f, thisPlayer == 1 ? (rotation - 90) : -(rotation - 90));
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
