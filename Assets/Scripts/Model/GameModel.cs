using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Tiro
    {
        public GameObject tiroObject;
        public float massa, densidade;
    }

    public class Player
    {
        int _escudo, _maxMovement;
        Queue<Tiro> _tiros;

        public Player()
        {
            _escudo = 3;
            _maxMovement = 5;
            _tiros = new Queue<Tiro>();
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

        public void Atirar(float angulo, float forca, float vento)
        {

        }

        public void Morrer()
        {

        }
    }

    public class GameModel : MonoBehaviour
    {

        public Player player1, player2;

        public GameModel()
        {
            player1 = new Player();
            player2 = new Player();
        }

    }
}

