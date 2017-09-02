using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {

        PlayerModel playerModel;


        private void Start()
        {
            playerModel = new PlayerModel();
        }

        public void CarregarTiros(GameObject[] tiros)
        {
            playerModel.CarregarTiros(tiros);
        }

        public void Morrer()
        {
            gameObject.SetActive(false);
        }

        public bool RetirarEscudo()
        {
            return playerModel.RetirarEscudo();
        }

        public GameObject Atirar()
        {
            return playerModel.Atirar();
        }

        public int GetCurrentMaxMovement()
        {
            return playerModel.MaxMovement;
        }

        public float AjustarAngulo(float angulo)
        {
            return playerModel.Angulo += angulo;
        }
        public float GetAngulo()
        {
            return playerModel.Angulo;
        }
        public int GetCurrentEscudo()
        {
            return playerModel.Escudo;
        }
        public Queue<GameObject> GetTiroFila()
        {
            return playerModel.GetTiroFila();
        }
        public bool IsDead()
        {
            return playerModel.IsDead;
        }

        public void ResetarPlayer()
        {
            playerModel = new PlayerModel();
        }

    }

}
