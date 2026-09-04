using UnityEngine;
using UnityEngine.UI;

public class PetQuestionManager : MonoBehaviour
{
    private PetController petController;

    [SerializeField] private QuestionListSO questionListSO;
    [SerializeField] private Canvas questionCanvas;
    [SerializeField] private QuestionBody questionBody;

    void Awake()
    {
        petController = GetComponent<PetController>();
    }

    void Start()
    {
        InvokeRepeating("TryAsk", 20f, 10f);
    }

    public void TryAsk()
    {
        if (petController.CurrentState != PetState.Idle) return;

        float randomNumber = Random.Range(0f, 100f);

        if (randomNumber <= GameManager.Instance.petQuestionChance)
        {
            // Gerar pergunta e enviar ao canva de resposta
            int randomIndex = Random.Range(0, questionListSO.questions.Count);
            QuestionSO randomQuestion = questionListSO.questions[randomIndex];

            questionBody.SetQuestion(randomQuestion);

            petController.ChangeState(PetState.Curious);
        }
        else
        {
            GameManager.Instance.petQuestionChance += GameManager.Instance.petFailedQuestionChanceIncrement;
        }
    }

    public void IncrementPetQuestionChance(float increment)
    {
        GameManager.Instance.petQuestionChance += increment;
        TryAsk();
    }

    public void ActivateQuestionCanvas()
    {
        questionCanvas.gameObject.SetActive(true);
        questionCanvas.GetComponent<QuestionCanvas>().ShowQuestionBody();
        petController.ChangeState(PetState.Listening);
    }
}
