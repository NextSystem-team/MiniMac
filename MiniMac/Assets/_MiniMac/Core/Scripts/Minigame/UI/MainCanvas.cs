using UnityEngine;

public class MainCanva : MonoBehaviour
{
    [SerializeField] private LineManager lineManager;
    [SerializeField] private GameObject pausePanel;

    public void TogglePausePanel()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        pausePanel.SetActive(!pausePanel.activeSelf);
        lineManager.canCreateLine = !lineManager.canCreateLine;
    }
}
