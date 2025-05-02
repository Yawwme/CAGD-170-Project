using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actor : MonoBehaviour
{
    //Between you and whoever is reading this comment, I HATE this script.
    //I know it's unprofessional, but the player isn't getting the source code. This script is the TF2 coconut.jpg meme except real
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
