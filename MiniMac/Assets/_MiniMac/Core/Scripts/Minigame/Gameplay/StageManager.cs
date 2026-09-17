using DG.Tweening;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private MainCanva mainCanva;

    public int dotsConnected;
    private float timePassed;

    private bool hasGetTheCoins;

    private void Update()
    {
        if (dotsConnected >= 2)
        {
            Time.timeScale = 1;

            if (!GameManager.Instance.hasPlayedMiniGame)
            {
                GameManager.Instance.money += 50;
                GameManager.Instance.playerScore += 50;
                GameManager.Instance.hasPlayedMiniGame = true;
                GameManager.Instance.IncrementPetQuestionChance(40);
            }
            else
            {
                if (!hasGetTheCoins)
                {
                    GameManager.Instance.playerScore += 5;
                    GameManager.Instance.money += 10;

                    hasGetTheCoins = true;
                    GameManager.Instance.IncrementPetQuestionChance(40);
                }
            }

            

            mainCanva.OpenCongratsPanel();
        }
    }
}
