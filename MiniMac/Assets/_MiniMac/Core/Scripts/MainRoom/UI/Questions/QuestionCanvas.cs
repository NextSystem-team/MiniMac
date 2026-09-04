using System.Collections;
using UnityEngine;

public class QuestionCanvas : MonoBehaviour
{
    [SerializeField] private QuestionBody questionBody;
    [SerializeField] private PetReaction petResponseBody;

    [SerializeField] private PetController petController;

    [SerializeField] private StreakCanva streakCanva;

    public void ShowQuestionBody()
    {
        questionBody.gameObject.SetActive(true);
        petResponseBody.gameObject.SetActive(false);
    }

    public void ShowPetResponseBody(string petResponse, Sprite petPose)
    {
        petResponseBody.SetPetResponse(petResponse, petPose);

        questionBody.gameObject.SetActive(false);
        petResponseBody.gameObject.SetActive(true);


        if (!GameManager.Instance.hasAnsweredQuestion)
        {
            GameManager.Instance.hasAnsweredQuestion = true;
            GameManager.Instance.playerScore += 50;
            GameManager.Instance.money += 50;
        }

        StartCoroutine(CloseCanvasAfterDelay(3f));
    }

    private IEnumerator CloseCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        questionBody.gameObject.SetActive(false);
        petResponseBody.gameObject.SetActive(false);

        petController.ChangeState(PetState.Idle);

        if (GameManager.Instance.hasAnsweredQuestion)
        {
            streakCanva.OpenStreakScreen();
        }

        gameObject.SetActive(false);
    }
}
