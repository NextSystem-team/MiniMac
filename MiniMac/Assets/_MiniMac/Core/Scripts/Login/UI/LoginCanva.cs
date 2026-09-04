using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginCanva : MonoBehaviour
{
    [SerializeField] private TMP_InputField loginField;
    [SerializeField] private Text emptyLoginFieldWarnText;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Text emptyPasswordFieldWarnText;

    [SerializeField] private string mainRoomScene;

    public void Login()
    {
        if (loginField.text == "" || loginField.text == null)
        {
            emptyLoginFieldWarnText.enabled = true;
        }
        else
        {
            emptyLoginFieldWarnText.enabled = false;
        }

        if (passwordField.text == "" || passwordField.text == null)
        {
            emptyPasswordFieldWarnText.enabled = true;
        }
        else
        {
            emptyPasswordFieldWarnText.enabled = false;
        }
        
        if (loginField.text != "" && loginField.text != null &&
            passwordField.text != "" && passwordField.text != null)
        {
            GameManager.Instance.playerName = loginField.text;

            SceneManager.LoadScene(mainRoomScene);
        }
    }
}
