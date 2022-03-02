using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject basicUI;
    public Image loadPanel;

    public void NewGame()
    {
        SceneManager.LoadScene("StartScene");
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