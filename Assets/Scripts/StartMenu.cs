using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneManager.LoadScene("GameJamScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
