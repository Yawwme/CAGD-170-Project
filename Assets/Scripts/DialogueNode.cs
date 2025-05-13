using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Node")]

/*
 * Date Created:
 * Authors:
 * Description: Originally from "How to Create a Dialogue System With Choices In Unity | Unity Game Dev Tutorial". THESE ARE THE NODES, THIS IS THE HEART OF THE GAME
 * Within the node, you're able to change who is speaking, the dialogue itself, etc
 */

public class DialogueNode : ScriptableObject
{
    /* 
    This is a fever dream to explain. Rather than dealing with the
    "Serialization depth limit 10 exceeded" warning from the video I watched (not good)
    One comment suggeested making the nodes scriptableObjects then make
    Nodes within nodes within nodes
    */


   
    [Header("speakerName is the Character's Name")]
    public string speakerName;

    [Header("What to show on screen. This is THE dialogue")]
    [TextArea(3, 8)] public string dialogueText;

    
    [Header("If you don't want a response for this node")]
    [Header("you can add another node to extend the scene")]
    public DialogueNode defaultNextNode;

    [Header("Gives the player the button choices to different rotues")]
    [Header("DO NOT TRY AND DO NEXT NODE AND RESPONSES!")]
    [Header("I NEVER TESTED IT, BUT DON'T TRY AND BREAK THIS!")]
    [Header("ResponseText is the text displayed on buttons")]

    public List<DialogueResponse> responses = new List<DialogueResponse>();

    public bool isBranching => responses != null && responses.Count > 0;

    public Sprite cubeChanSprite;
    public bool showCubeChan;
    public bool playSound;

    //Old code, go look and gawk.
    internal bool IsLastNode()
    {
        return responses.Count <= 0;
    }

}



