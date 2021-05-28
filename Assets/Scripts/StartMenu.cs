using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject creditsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameJamScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCreditsMenu()
    {
        startPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCreditsMenu()
    {
        creditsPanel.SetActive(false);
        startPanel.SetActive(true);
    }
}
