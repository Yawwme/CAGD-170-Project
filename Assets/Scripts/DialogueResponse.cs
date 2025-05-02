using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public DialogueNode nextNode;
    public UnityEvent onSelected; //Shoutout to that how to make a dating sim youtube series for the event tutorial 
    public UnityEvent onClick; //testing

}
