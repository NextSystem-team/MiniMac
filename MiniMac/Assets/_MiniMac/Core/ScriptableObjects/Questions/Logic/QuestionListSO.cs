using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionList", menuName = "Scriptable Objects/Question List")]
public class QuestionListSO : ScriptableObject
{
    public List<QuestionSO> questions;
}
