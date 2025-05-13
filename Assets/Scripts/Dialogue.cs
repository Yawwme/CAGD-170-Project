using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make the Conversation node first then dialogue nodes
[CreateAssetMenu(menuName = "Dialogue/Conversation")]

/*
 * Date Created:
 * Authors: Jann Morales and and Ricky Pardo
 * Description: Originally from "How to Create a Dialogue System With Choices In Unity | Unity Game Dev Tutorial". This creates the dialogue ScriptableObject 
 * It makes nodes, that simple.
 */

public class Dialogue : ScriptableObject
{
    public DialogueNode rootNode;
}