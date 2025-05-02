using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make the Conversation node first then dialogue nodes
[CreateAssetMenu(menuName = "Dialogue/Conversation")]

public class Dialogue : ScriptableObject
{
    public DialogueNode rootNode;
}