using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public DialogueNode nextNode;
    public UnityEvent onSelected; //Shoutout to that how to make a dating sim youtube series for the event tutorial 

}
