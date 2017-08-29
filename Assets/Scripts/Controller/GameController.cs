using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class GameController : MonoBehaviour {

    GameModel gameModel;

    private void Start()
    {
        Inicializar();
    }

    public void Inicializar()
    {
        gameModel = new GameModel();
    }

}
