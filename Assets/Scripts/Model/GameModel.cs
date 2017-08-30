using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{

    public class GameModel
    {

        struct Vento
        {
            public bool direcao;
            public float forca;
        }

        Vento vento;

        int _playerSide;

        public PlayerModel player1, player2;



        public GameModel(GameObject[] tiros)
        {
            _playerSide = 1;
            vento = new Vento() { direcao = false, forca = 0f };
            player1 = new PlayerModel(tiros);
            player2 = new PlayerModel(tiros);
        }

        public void ChangeVento()
        {

        }

        public void ChangeSide()
        {
            _playerSide = _playerSide == 1 ? 2 : 1;
        }

        public int GetPlayerSide()
        {
            return _playerSide;
        }

        public int GetCurrentMaxMovement()
        {
            if (_playerSide == 1)
                return player1.MaxMovement;
            else
                return player2.MaxMovement;
        }

        public GameObject Atirar()
        {
            if (_playerSide == 1)
                return player1.Atirar();
            else
                return player2.Atirar();
        }

    }
}

