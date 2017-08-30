using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Model
{
    public class PlayerModel
    {

        int _escudo, _maxMovement;
        Queue<GameObject> _tiroFila;
        GameObject[] tiro;

        public PlayerModel(GameObject[] tiros)
        {
            _escudo = 3;
            _maxMovement = 5;
            tiro = tiros;
            _tiroFila = new Queue<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                _tiroFila.Enqueue(tiros[Random.Range(0, (tiros.Length - 1))]);
            }
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

        public void RetirarEscudo()
        {
            if (_escudo > 0)
                _escudo--;
            else
                Morrer();
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

        public void Morrer()
        {

        }

        public Queue<GameObject> GetTiroFila()
        {
            return _tiroFila;
        }
    }

}
