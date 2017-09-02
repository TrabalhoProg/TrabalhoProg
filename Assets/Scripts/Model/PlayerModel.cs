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
        bool _isDead;

        public PlayerModel()
        {
            _isDead = false;
            _escudo = 0;
            _maxMovement = 10;
            _tiroFila = new Queue<GameObject>();
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
            if (_escudo < 0)
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

        public GameObject Atirar()
        {
            _tiroFila.Enqueue(tiro[Random.Range(0, tiro.Length)]);
            GameObject ret = _tiroFila.Dequeue();
            return ret;
        }

        public void CarregarTiros(GameObject[] tiros)
        {
            tiro = tiros;
            for (int i = 0; i < 3; i++)
            {
                _tiroFila.Enqueue(tiros[Random.Range(0, tiros.Length)]);
            }
        }

        public Queue<GameObject> GetTiroFila()
        {
            return _tiroFila;
        }
    }

}
