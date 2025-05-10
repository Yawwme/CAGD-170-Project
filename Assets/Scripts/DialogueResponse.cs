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

    [Header("TYPE THE SCENE YOU WANT TO LOAD HERE!")]
    [Header("DO NOT MESS UP THE SPELLING!")]
    public string loadScene;

    [Header("Throw in the amazing cube-chan here!")]
    public Sprite sprite;

    public UnityEvent onClick; //Might get removed if it does nothing. 
    //public AudioSource audioSource;

}
