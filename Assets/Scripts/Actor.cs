using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actor : MonoBehaviour
{
    //Attach this script to an empty GameObject
    //Honestly, this script is pretty useless, but I'm afraid of things breaking if it gets removed as I built
    //the dialogue system around youtube videos. This script is the TF2 coconut.jpg meme except real

    [Header("Put the Conversation Node here!")]
    public Dialogue Dialogue;

    private void Update()
    {
        //Only start dialogue if nothing is up already
        //At some point, get rid of the Space to SHOW the dialogue.
        if (!DialogueManager.Instance.IsDialogueActive() && Input.GetKeyDown(KeyCode.Space))
        {
            SpeakTo();
        }
    }


   public void SpeakTo()
{
    DialogueManager.Instance.StartDialogue(Dialogue);
}

}
