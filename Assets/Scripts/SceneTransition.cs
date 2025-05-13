using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Author: Jann Morales
 * Date Created: 5/1/2025
 * Description: While this script is useless, it was supposed to... change scenes.
 * Wow, who would've guessed.
 */

//Transition skript
//Shoutout to UnityDocumentation
public class SceneTransition : MonoBehaviour
{
    public void SceneChange(int buildIndex)
    {
        Debug.Log("sceneName loading" + buildIndex);
        SceneManager.LoadScene(buildIndex);
    }

}
