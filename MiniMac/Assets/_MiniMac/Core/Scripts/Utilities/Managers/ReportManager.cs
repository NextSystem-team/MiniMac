using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ReportManager : MonoBehaviour
{
    public static ReportManager Instance;

    private List<string> answerHistory = new List<string>();

    public bool hasMadeDailyRoutine;

    public int currentRoutineStreak;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SaveAnswer(QuestionSO question, string chosenAnswer)
    {
        string log = $"[{System.DateTime.Now:HH:mm}] \"{question.questionText}\": {chosenAnswer}";
        answerHistory.Add(log);
        Debug.Log("Resposta salva: " + log);
    }

    public void UpdateReportScreen(TextMeshProUGUI reportTextUI)
    {
        reportTextUI.text = $"<b>Registro do Dia de {GameManager.Instance.playerName}:</b>\n\n";

        if (hasMadeDailyRoutine)
        {
            reportTextUI.text += "Rotina diária concluída!\n\n";
            reportTextUI.text += $"Sequência atual de rotinas diárias feitas: {currentRoutineStreak}\n\n";
        }
        else
        {
            reportTextUI.text += "Rotina diária não concluída.\n\n";
        }

        if (answerHistory.Count == 0)
        {
            reportTextUI.text += "Nenhuma resposta registrada.";
            return;
        }
        else
        {
            reportTextUI.text += "<b>Histórico de Respostas:</b>\n";
            foreach (string item in answerHistory)
            {
                reportTextUI.text += item + "\n";
            }
        }
    }
}