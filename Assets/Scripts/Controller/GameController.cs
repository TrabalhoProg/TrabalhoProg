﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Controller
{
    public class GameController : MonoBehaviour
    {

        public GameObject[] tiros, pickupItems;
        public AudioSource audioSrc;

        public Transform[] pickUpSpawnRefEsq, pickUpSpawnRefDir;
        public int pickUpSpawnCount = 4;

        GameModel gameModel;
        //Stack<GameModel> partidas;

        PlayerController player1Controller, player2Controller;

        float _playerMovement;
        float _currentPlayerForce;
        float _currentVento;

        public float playerSpeed, shotForce;
        public bool isShotCharging, hasControl;

        public float playerMaxForce, playerMinForce;

        Vector3 player1RefPosition, player2RefPosition;

        public float PlayerMovement
        {
            get
            {
                return _playerMovement;
            }

            set
            {
                if (value < 0)
                    _playerMovement = 0;
                else
                    _playerMovement = value;
            }
        }
        public float CurrentPlayerForce
        {
            get
            {
                return _currentPlayerForce;
            }

            set
            {
                if (value > playerMaxForce)
                {
                    _currentPlayerForce = playerMaxForce;
                }
                else if (value < 0)
                {
                    _currentPlayerForce = 0;
                }
                else
                {
                    _currentPlayerForce = value;
                }
            }
        }

        private void Start()
        {
            //playerMaxForce = 1000f;
            shotForce = 0.1f;
            playerSpeed = 100;

            hasControl = true;
            Inicializar();
        }

        public void EscolherPosição(Vector3 position)
        {
            if (GetPlayerSide() == 1)
            {
                player1RefPosition = new Vector3(position.x, position.y, 0f);
                //player1Controller.gameObject.SetActive(true);
                gameModel.ChangeSide();
            }
            else
            {
                player2RefPosition = new Vector3(position.x, position.y, 0f);
                //player2Controller.gameObject.SetActive(true);
                gameModel.ChangeSide();
                gameModel.ChangeGameState();
                IniciarPartida();
            }
        }

        public void Inicializar()
        {

            if (player1Controller == null)
            {
                player1Controller = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
            }
            if (player2Controller == null)
            {
                player2Controller = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();
            }
            player1Controller.ResetarPlayer();
            player1Controller.CarregarTiros(tiros);
            //player1Controller.gameObject.SetActive(true);
            player1Controller.gameObject.SetActive(false);

            player2Controller.ResetarPlayer();
            player2Controller.CarregarTiros(tiros);
            //player2Controller.gameObject.SetActive(true);
            player2Controller.gameObject.SetActive(false);

            gameModel = new GameModel();
            //partidas = new Stack<GameModel>();
            isShotCharging = false;
            CurrentPlayerForce = 0f;
            PlayerMovement = (float)GetCurrentMaxMovement() * 100;



            hasControl = false;
        }

        void IniciarPartida()
        {

            player1Controller.gameObject.transform.position = player1RefPosition;
            player2Controller.gameObject.transform.position = player2RefPosition;
            player1Controller.gameObject.SetActive(true);
            player2Controller.gameObject.SetActive(true);
            SetPickUpItems();
            StartCoroutine(IniciarPartidaTime());
        }

        IEnumerator IniciarPartidaTime()
        {
            yield return new WaitForSeconds(1);
            hasControl = true;
        }

        public void ChangePlayerSide()
        {
            gameModel.ChangeSide();
        }

        public int GetPlayerSide()
        {
            return gameModel.GetPlayerSide();
        }

        public int GetCurrentMaxMovement()
        {
            if (gameModel.GetPlayerSide() == 1)
            {
                return player1Controller.GetCurrentMaxMovement();
            }
            else
            {
                return player2Controller.GetCurrentMaxMovement();
            }
        }
        public float GetCurrentAngulo()
        {
            if (gameModel.GetPlayerSide() == 1)
            {
                return player1Controller.GetAngulo();
            }
            else
            {
                return player2Controller.GetAngulo();
            }
        }

        public GameObject Atirar()
        {
            if (gameModel.GetPlayerSide() == 1)
            {
                return player1Controller.Atirar();
            }
            else
            {
                return player2Controller.Atirar();
            }
        }

        public bool RemoveEscudo(int player)
        {
            if (player == 1)
            {
                return player1Controller.RetirarEscudo();
            }
            else
            {
                return player2Controller.RetirarEscudo();
            }
        }


        public void IniciarNovoTurno()
        {
            hasControl = true;
            ChangePlayerSide();
            isShotCharging = false;
            CurrentPlayerForce = 0f;
            PlayerMovement = (float)GetCurrentMaxMovement() * 100;
            ChangeWind();
        }

        IEnumerator IniciarNovaPartidaWait()
        {

            yield return new WaitForSeconds(2);
            ChangeGameState();
            player1Controller.ResetarPlayer();
            player1Controller.CarregarTiros(tiros);
            player2Controller.ResetarPlayer();
            player2Controller.CarregarTiros(tiros);

            isShotCharging = false;
            CurrentPlayerForce = 0f;
            PlayerMovement = (float)GetCurrentMaxMovement() * 100;

            player1Controller.gameObject.SetActive(false);
            player2Controller.gameObject.SetActive(false);

            hasControl = false;

        }

        public void ChangeWind()
        {
            _currentVento = gameModel.ChangeVento();
            Physics2D.gravity = new Vector2(_currentVento, Physics2D.gravity.y);
            //Debug.Log(_currentVento);
        }

        public float GetCurrentVento()
        {
            return gameModel.GetVento();
        }

        public int GetGameState()
        {
            return gameModel.GameState;
        }
        public void ChangeGameState()
        {
            gameModel.ChangeGameState();
        }
        public int[] GetCurrentScore()
        {
            return gameModel.GetScore();
        }
        public int AcabarPartida(int winner)
        {
            int temp = gameModel.EndPartida(winner);
            if (temp == 0)
            {
                StartCoroutine(IniciarNovaPartidaWait());
            }
            return temp;
        }
        public void AdicionarEscudo()
        {
            if (gameModel.GetPlayerSide() == 1)
            {
                player1Controller.AdicionarEscudo();
            }
            else
            {
                player2Controller.AdicionarEscudo();
            }
        }
        public void AdicionarMovimento()
        {
            if (gameModel.GetPlayerSide() == 1)
            {
                player1Controller.AdicionarMovimento();
            }
            else
            {
                player2Controller.AdicionarMovimento();
            }
        }
        public void AdicionarDano()
        {
            if (gameModel.GetPlayerSide() == 1)
            {
                player1Controller.AdicionarDano();
            }
            else
            {
                player2Controller.AdicionarDano();
            }
        }

        public bool IsDoubleDano()
        {
            if (gameModel.GetPlayerSide() == 1)
            {
                return player1Controller.IsDoubleDano();
            }
            else
            {
                return player2Controller.IsDoubleDano();
            }
        }

        public GameObject ReturnRandomPickUp()
        {
            float maxRange = 300f;
            float temp = Random.Range(-maxRange, maxRange);
            temp = Mathf.Abs(temp);
            for (int i = 0; i < 3; i++)
            {
                if (temp < (i * 100f))
                {
                    return pickupItems[i];
                }
            }
            return pickupItems[0];
        }
        void SetPickUpItems()
        {
            //int tempCount = pickUpSpawnCount;
            //for (int i = 0; i < pickUpSpawnCount; i++)
            //{
            //    int tempIndex = Random.Range(0, pickUpSpawnRefEsq.Length);
            //    for (int a = 0; a < pickUpSpawnRefEsq.Length; a++)
            //    {
            //        if (tempIndex==a)
            //        {

            //            break;
            //        }
            //    }
            //}
        }

        public void PlaySound(AudioClip audio)
        {
            audioSrc.PlayOneShot(audio);
        }

        public void Restart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}

