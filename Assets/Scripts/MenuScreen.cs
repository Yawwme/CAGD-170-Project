using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Author: Ricky Pardo
 * Date Created 4 / 1 / 2025
 * Description: main menu screen UI and map loading (we copied it over for the main menu)
 * 
 */
public class MenuScreen : MonoBehaviour
{
    public void PlayButtonPressed(string buildName)
    {
        SceneManager.LoadScene(buildName);


    }
   
    public void QuitButtonPressed()
    {
        print("Quit Game");
        EditorApplication.ExitPlaymode();
        Application.Quit();
    }
}
