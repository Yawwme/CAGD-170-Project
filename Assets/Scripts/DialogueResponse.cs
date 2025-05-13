using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]

/*
 * Date Created: 4/24/25
 * Authors: Jann Morales and and Ricky Pardo
 * Description: Originally from "How to Create a Dialogue System With Choices In Unity | Unity Game Dev Tutorial". Within the nodes, you are able to create
 * responses, these are the options within the responses. 
 */

public class DialogueResponse
{
    public string responseText;
    public DialogueNode nextNode;
    public UnityEvent onSelected; //Shoutout to that how to make a dating sim youtube series for the event tutorial 

    [Header("TYPE THE SCENE YOU WANT TO LOAD HERE!")]
    [Header("DO NOT MESS UP THE SPELLING!")]
    public string loadScene;

    [Header("This doesn't work, but feel free to throw in sprites")]
    [Header("The Cube Chan Sprite works though, not this.")]
    public Sprite sprite;



    public UnityEvent onClick;
    //public AudioSource audioSource; //I hate type mismatch
    //public bool playSoundResponse; This doesn't work, I tried

}
