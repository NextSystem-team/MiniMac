using UnityEngine;
using UnityEngine.UI;

public class QuestionBody : MonoBehaviour
{
    [SerializeField] private Text questionTitle;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] QuestionCanvas canvas;

    public void SetQuestion(QuestionSO question)
    {
        questionTitle.text = question.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].onClick.RemoveAllListeners();

            if (i < question.answerOptions.Length)
            {
                answerButtons[i].GetComponentInChildren<Text>().text = question.answerOptions[i].answerText;
                string petResponse = question.answerOptions[i].petResponse;
                string playerResponse = question.answerOptions[i].answerText;
                Sprite petPose = question.answerOptions[i].petPose;
                answerButtons[i].onClick.AddListener(() => OnAnswerClick(question, playerResponse, petResponse, petPose));
                answerButtons[i].gameObject.SetActive(true);
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnAnswerClick(QuestionSO question, string playerResponse, string petResponse, Sprite petPose)
    {
        ReportManager.Instance.SaveAnswer(question, playerResponse);
        canvas.ShowPetResponseBody(petResponse, petPose);
    }
}
