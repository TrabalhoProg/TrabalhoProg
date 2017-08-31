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

        public GameModel()
        {
            _playerSide = 1;
            vento = new Vento() { direcao = false, forca = 0f };
            ChangeVento();
        }

        public void ChangeVento()
        {
            //set direcao
            float direcao = Random.Range(0f, 100f);
            if (direcao < 50)
                vento.direcao = false;
            else
                vento.direcao = true;

            //setforca
            vento.forca = Random.Range(0f, 100f);
        }

        public void ChangeSide()
        {
            _playerSide = _playerSide == 1 ? 2 : 1;
        }

        public int GetPlayerSide()
        {
            return _playerSide;
        }

    }
}

