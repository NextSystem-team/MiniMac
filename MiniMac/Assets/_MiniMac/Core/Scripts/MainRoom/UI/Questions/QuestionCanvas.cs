using System.Collections;
using DG.Tweening;
using UnityEngine;

public class QuestionCanvas : MonoBehaviour
{
    [SerializeField] private QuestionBody questionBody;
    [SerializeField] private PetReaction petResponseBody;

    [SerializeField] private PetController petController;

    [SerializeField] private StreakCanva streakCanva;

    void OnEnable()
    {
        GetComponent<CanvasGroup>().DOFade(1f, 0.8f);
    }

    void OnDisable()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    public void ShowQuestionBody()
    {
        questionBody.gameObject.SetActive(true);
        petResponseBody.gameObject.SetActive(false);
    }

    public void ShowPetResponseBody(string petResponse, AnimationClip petAnimation)
    {
        petResponseBody.SetPetResponse(petResponse, petAnimation);

        questionBody.gameObject.SetActive(false);
        petResponseBody.gameObject.SetActive(true);


        if (!GameManager.Instance.hasAnsweredQuestion)
        {
            GameManager.Instance.hasAnsweredQuestion = true;
            GlobalEvents.ConcludeQuestionMission?.Invoke();
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
