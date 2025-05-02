using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 
 * Date Created: 5/1/2025
 * 
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
