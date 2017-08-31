using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class GameController : MonoBehaviour
{

    public GameObject[] tiros;

    GameModel gameModel;

    PlayerController player1Controller, player2Controller;

    float _playerMovement;
    float _currentPlayerForce;

    public float playerSpeed, shotForce;
    public bool isShotCharging,hasControl;

    public float playerMaxForce;
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
        playerMaxForce = 800f;
        shotForce = 10f;
        playerSpeed = 100;

        hasControl = true;

        Inicializar();
    }

    public void Inicializar()
    {
        player1Controller = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
        player1Controller.CarregarTiros(tiros);

        player2Controller = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();
        player2Controller.CarregarTiros(tiros);
        
        gameModel = new GameModel();
        isShotCharging = false;
        CurrentPlayerForce = 0f;
        PlayerMovement = (float)GetCurrentMaxMovement() * 100;
        //hasControl = false;
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

    public void RemoveEscudo(int player)
    {
        if (player == 1)
        {
            if (player1Controller.RetirarEscudo())
            {
                player1Controller.Morrer();
            }
        }
        else
        {
            if (player2Controller.RetirarEscudo())
            {
                player2Controller.RetirarEscudo();
            }
        }
    }

    public float AjustarAngulo(float angulo)
    {
        if (gameModel.GetPlayerSide()==1)
        {
            return player1Controller.AjustarAngulo(angulo);
        }
        else
        {
           return  player2Controller.AjustarAngulo(angulo);
        }
    }
    
    public float GetCurrentAngulo()
    {
        if (gameModel.GetPlayerSide()==1)
        {
            return player1Controller.GetAngulo();
        }
        else
        {
            return player2Controller.GetAngulo();
        }
    }

}
