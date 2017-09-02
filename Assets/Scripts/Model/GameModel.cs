using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{

    public class GameModel
    {
        float _vento;
        int _playerSide, _gameState, _gameTurn;
        int[] _score;

        public int GameState
        {
            get { return _gameState; }
        }

        

        public int GameTurn
        {
            get { return _gameTurn; }
        }

        public GameModel()
        {
            _score = new int[2];
            _vento = 0f;
            _playerSide = _gameState = _gameTurn = 1;
        }

        public float ChangeVento()
        {
            float random = Random.Range(0f, 200f);
            if (random <= 100f)
            {
                return _vento;
            }
            else if (random < 150)
            {
                return _vento = Random.Range(-4f, 4f);
            }
            else
            {
                return _vento = 0;
            }
        }
        public float GetVento()
        {
            return _vento;
        }

        public void ChangeSide()
        {
            if (GameState!=3)
            {
                _playerSide = _playerSide == 1 ? 2 : 1;
            }
        }


        public int GetPlayerSide()
        {
            return _playerSide;
        }

        public int EndPartida(int winner)
        {
            if (winner == 1)
            {
                _score[0]++;
                if (_score[0] == 2)
                {
                    _gameState = 4;
                    return 1;
                }
            }
            else
            {
                _score[1]++;
                if (_score[0] == 2)
                {
                    _gameState = 4;
                    return 2;
                }
            }
            _gameTurn++;
            _playerSide = 1;
            return 0;
        }

        public void ChangeGameState()
        {
            _gameState++;
            if (_gameState > 3)
            {
                _gameTurn = 1;
                _gameState = 1;
            }
        }

        public int[] GetScore()
        {
            return _score;
        }
    }
}

