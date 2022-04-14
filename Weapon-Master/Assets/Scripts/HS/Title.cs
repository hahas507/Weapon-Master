using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject basicUI;
    public Image inputNamePanel;
    public Image loadPanel;

    public void NewGame()
    {
        basicUI.SetActive(false);
        inputNamePanel.gameObject.SetActive(true);
    }

    public void LoadGame()
    {
        basicUI.SetActive(false);
        loadPanel.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}