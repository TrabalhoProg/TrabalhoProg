using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Controller;

namespace View
{
    public class GameView : MonoBehaviour
    {

        public GameController gameController;

        public Text movementText, anguloText, ventoText, generalUI, player1UI, player2UI;
        public Slider forcaSlider;
        public Animator sliderAnimator;
        public Button RestartBtn;
        public GameObject BottomUI,TopUi;

        private void Start()
        {
            //sliderAnimator = forcaSlider.gameObject.GetComponentInChildren<Animator>();
            forcaSlider.maxValue = gameController.playerMaxForce;
            UpdateUI();
        }

        private void LateUpdate()
        {
            sliderAnimator.SetFloat("Forca",gameController.CurrentPlayerForce);
            UpdateUI();
        }

        void UpdateUI()
        {
            if (gameController.GetGameState() == 1)
            {
                BottomUI.SetActive(false);
                TopUi.SetActive(false);
                RestartBtn.gameObject.SetActive(false);
                player1UI.gameObject.SetActive(false);
                player2UI.gameObject.SetActive(false);
                ventoText.gameObject.SetActive(false);
                anguloText.gameObject.SetActive(false);
                movementText.gameObject.SetActive(false);
                forcaSlider.gameObject.SetActive(false);
                generalUI.gameObject.SetActive(true);
                generalUI.text = gameController.GetPlayerSide() == 1 ? "Escolha a posição do Player 1" : "Escolha a posição do Player 2";
                Vector3 mouseRef = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetMouseButtonDown(0)
                    && gameController.GetPlayerSide() == 1
                    && mouseRef.x > -16f
                    && mouseRef.x < 0f
                    && mouseRef.y > -8f
                    && mouseRef.y < 8f)
                {
                    gameController.EscolherPosição(mouseRef);
                }
                else if (Input.GetMouseButtonDown(0)
                    && gameController.GetPlayerSide() == 2
                    && mouseRef.x > 0f
                    && mouseRef.x < 16f
                    && mouseRef.y > -8f
                    && mouseRef.y < 8f)
                {
                    gameController.EscolherPosição(mouseRef);
                }
            }
            else if (gameController.GetGameState() == 2)
            {
                int[] temp = gameController.GetCurrentScore();
                player1UI.text = temp[0].ToString();
                player2UI.text = temp[1].ToString();
                ventoText.text = ReturnVento(gameController.GetCurrentVento());
                anguloText.text = Mathf.Round(gameController.GetCurrentAngulo()).ToString() + "°";
                movementText.text = Mathf.Round(gameController.PlayerMovement).ToString();
                forcaSlider.value = gameController.CurrentPlayerForce;
                BottomUI.SetActive(true);
                TopUi.SetActive(true);
                player1UI.gameObject.SetActive(true);
                player2UI.gameObject.SetActive(true);
                RestartBtn.gameObject.SetActive(false);
                generalUI.gameObject.SetActive(false);
                ventoText.gameObject.SetActive(true);
                anguloText.gameObject.SetActive(true);
                movementText.gameObject.SetActive(true);
                forcaSlider.gameObject.SetActive(true);
            }
            else
            {
                int[] temp = gameController.GetCurrentScore();
                if (temp[0] == 2)
                {
                    generalUI.text = "Game Over\nPlayer 1 é o vencedor";
                    RestartBtn.gameObject.SetActive(true);
                    generalUI.gameObject.SetActive(true);

                }
                else if (temp[1] == 2)
                {
                    generalUI.text = "Game Over\nPlayer 2 é o vencedor";
                    RestartBtn.gameObject.SetActive(true);
                    generalUI.gameObject.SetActive(true);
                }
                else
                {
                    if (gameController.GetPlayerSide() == 1)
                        generalUI.text = "Player 1 ganhou essa rodada";
                    else
                        generalUI.text = "Player 2 ganhou essa rodadr";
                    generalUI.gameObject.SetActive(true);
                }
            }
        }

        

        public string ReturnVento(float vento)
        {
            if (vento == 0)
            {
                return "|";
            }
            else if (vento > 0)
            {
                if (vento > 2)
                {
                    return ">>>";
                }
                else if (vento > 1)
                {
                    return ">>";
                }
                else
                {
                    return ">";
                }
            }
            else
            {
                if (vento < -2)
                {
                    return "<<<";
                }
                else if (vento < -1)
                {
                    return "<<";
                }
                else
                {
                    return "<";
                }
            }
        }
    }
}

