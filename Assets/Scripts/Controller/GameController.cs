using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class GameController : MonoBehaviour
{

    public GameObject[] tiros;

    GameModel gameModel;

    float _playerMovement;
    float _anguloArma;
    float _currentPlayerForce;

    public float playerSpeed, shotForce;
    public float player1LastAngulo, player2LastAngulo;
    public bool isShotCharging;

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
    public float AnguloArma
    {
        get
        {
            return _anguloArma;
        }

        set
        {
            if (value > 90)
                _anguloArma = 90;
            else if (value < 0)
                _anguloArma = 0;
            else _anguloArma = value;
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
        Inicializar();
    }

    public void Inicializar()
    {
        gameModel = new GameModel(tiros);
        isShotCharging = false;
        CurrentPlayerForce = 0f;
        AnguloArma = player1LastAngulo = player2LastAngulo = 0f;
        PlayerMovement = (float)gameModel.GetCurrentMaxMovement() * 100;
    }


    public void ChangePlayerSide()
    {
        if (gameModel.GetPlayerSide() == 1)
        {
            player1LastAngulo = Mathf.Abs(AnguloArma);
        }
        else
        {
            player2LastAngulo = Mathf.Abs(AnguloArma);
        }
        gameModel.ChangeSide();
    }

    public int GetPlayerSide()
    {
        return gameModel.GetPlayerSide();
    }
    public int GetCurrentMaxMovement()
    {
        return gameModel.GetCurrentMaxMovement();
    }

    public GameObject Atirar()
    {
        return gameModel.Atirar();
    }
}
