using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Não esqueça do namespace!

public class QuestionBody : MonoBehaviour
{
    [SerializeField] private Text questionTitle;
    [SerializeField] private CanvasGroup answersGroup;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] QuestionCanvas canvas;

    [SerializeField] private float textReavelTime = 3f;
    [SerializeField] private float fadeButtonsTime = 0.85f;

    private string fullQuestionText = "";
    private Sequence introSequence;

    public void SetQuestion(QuestionSO question)
    {
        fullQuestionText = question.questionText;
        
        questionTitle.text = ""; 
        answersGroup.alpha = 0f;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].onClick.RemoveAllListeners();

            if (i < question.answerOptions.Length)
            {
                answerButtons[i].GetComponentInChildren<Text>().text = question.answerOptions[i].answerText;
                string petResponse = question.answerOptions[i].petResponse;
                string playerResponse = question.answerOptions[i].answerText;
                AnimationClip petAnimation = question.answerOptions[i].petAnimation;
                answerButtons[i].onClick.AddListener(() => OnAnswerClick(question, playerResponse, petResponse, petAnimation));
                answerButtons[i].gameObject.SetActive(true);
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(fullQuestionText)) return;

        introSequence?.Kill();

        questionTitle.text = "";
        answersGroup.alpha = 0f;

        introSequence = DOTween.Sequence();

        introSequence.Append(questionTitle.DOText(fullQuestionText, textReavelTime).SetEase(Ease.Linear));

        introSequence.Append(answersGroup.DOFade(1f, fadeButtonsTime));
    }

    private void OnDisable()
    {
        introSequence?.Kill();
        
        answersGroup.alpha = 1f;
    }

    private void OnAnswerClick(QuestionSO question, string playerResponse, string petResponse, AnimationClip petAnimation)
    {
        ReportManager.Instance.SaveAnswer(question, playerResponse);
        canvas.ShowPetResponseBody(petResponse, petAnimation);
    }
}