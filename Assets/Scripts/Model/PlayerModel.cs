using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Model
{
    public class PlayerModel
    {

        int _escudo, _maxMovement;
        float _angulo;
        Queue<GameObject> _tiroFila;
        GameObject[] tiro;
        bool _isDead, _nextAumentarShotDano;

        int doubleDanoCount = 0;

        public PlayerModel()
        {
            _isDead = false;
            _escudo = 3;
            _maxMovement = 3;
            _tiroFila = new Queue<GameObject>();
            doubleDanoCount = 0;
        }

        public int Escudo
        {
            get
            {
                return _escudo;
            }
        }

        public int MaxMovement
        {
            get
            {
                return _maxMovement;
            }
        }

        public float Angulo
        {
            get
            {
                return _angulo;
            }

            set
            {
                if (value > 0 && value < 60)
                {
                    _angulo = value;
                }
                else if (value < 0)
                {
                    _angulo = 0;
                }
                else if (value > 60)
                {
                    _angulo = 60f;
                }

            }
        }

        public bool IsDead
        {
            get
            {
                return _isDead;
            }
        }

        public bool RetirarEscudo()
        {
            if (_escudo > 0)
            {
                _escudo--;
                return false;
            }
            else
            {
                _isDead = true;
                return true;
            }
        }

        public void AumentarEscudo()
        {
            if (_escudo >= 0 && _escudo < 5)
            {
                _escudo++;
            }
        }

        public void RetirarMaxMovement()
        {
            if (MaxMovement > 0)
            {
                _maxMovement--;
            }
        }

        public void AumentarMaxMovement()
        {
            _maxMovement++;
        }

        public bool NextAumentarShotDano { get { return _nextAumentarShotDano; } }

        public GameObject Atirar()
        {
            if (doubleDanoCount > 0)
            {
                _nextAumentarShotDano = false;
                doubleDanoCount = 0;
            }
            if (doubleDanoCount == 0 && _nextAumentarShotDano)
            {
                doubleDanoCount++;
            }
            _tiroFila.Enqueue(tiro[ReturnRandomTiroIndex()]);
            GameObject ret = _tiroFila.Dequeue();
            return ret;
        }

        public void CarregarTiros(GameObject[] tiros)
        {
            tiro = tiros;
            for (int i = 0; i < 3; i++)
            {
                _tiroFila.Enqueue(tiros[ReturnRandomTiroIndex()]);
            }
        }

        public Queue<GameObject> GetTiroFila()
        {
            return _tiroFila;
        }
        public int ReturnRandomTiroIndex()
        {
            float maxRange = 100f * tiro.Length;
            float temp = Random.Range(-maxRange, maxRange);
            temp = Mathf.Abs(temp);
            for (int i = 0; i < (tiro.Length - 1); i++)
            {
                if (temp < ((i + 1) * 100f))
                {
                    return i;
                }
            }
            return tiro.Length - 1;
        }

        public void AdicionarDano()
        {
            _nextAumentarShotDano = true;
        }

    }

}
