using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenu;
    public Text countDown;
    
    void Update()
    {
        Time.timeScale = gamePaused ? 0.0f : 1.0f;
        if(Input.GetKeyDown(KeyCode.Escape) && !gamePaused) Pause();
    }

    void Pause(){
        gamePaused = true;
        pauseMenu.SetActive(true);
    }

    public void Resume(){
        pauseMenu.SetActive(false);
        StartCoroutine(CountDown());
    }

    public void ToVillage(){
        gamePaused = false;
        pauseMenu.SetActive(false);
        //SceneManager.LoadScene("VillageScene");
    }

    public void ToTitle(){
        gamePaused = false;
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator CountDown(){
        countDown.gameObject.SetActive(true);
        for(int i=3;i>0;i--){
            countDown.text = i.ToString();
            yield return new WaitForSecondsRealtime(1.0f);
        }
        countDown.gameObject.SetActive(false);
        gamePaused = false;
    }
}
