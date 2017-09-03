using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class PickUpController : MonoBehaviour
    {
        GameController gameController;

        public float amplitude;
        public float period;

        public GameObject Explosion;

        float sineWaveTime = 0, theta;
        Vector3 startPosition;

        public string Bonus;

        void Start()
        {
            startPosition = transform.position;
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        // Update is called once per frame
        void Update()
        {
            sineWaveTime += Time.deltaTime;
            theta = sineWaveTime / period;
            theta = amplitude * Mathf.Sin(theta);
            Vector3 temp = (startPosition + Vector3.up * theta);
            transform.transform.position = temp;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (Bonus)
            {
                case "escudo":
                    gameController.AdicionarEscudo();
                    break;
                case "movimento":
                    gameController.AdicionarMovimento();
                    break;
                case "dano":
                    gameController.AdicionarDano();
                    break;
                default:
                    break;
            }
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}

