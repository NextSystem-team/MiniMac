using UnityEngine;

[System.Serializable]
public class AnswerOption
{
    [TextArea(2, 3)]
    public string answerText;

    [TextArea(2, 3)]
    public string petResponse;

    public Sprite petPose;
    public AnimationClip petAnimation;
}

[CreateAssetMenu(fileName = "Question", menuName = "Scriptable Objects/Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(3, 5)]
    public string questionText;

    public AnswerOption[] answerOptions;
}
