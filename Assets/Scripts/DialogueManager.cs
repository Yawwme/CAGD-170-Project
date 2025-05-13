using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;

/*
 * Date Created: 4/
 * Authors: Jann Morales and Ricky Pardo
 * Description: Originally from "How to Create a Dialogue System With Choices In Unity | Unity Game Dev Tutorial" but changed so much to the point where it is barely the same script
 * This handles ALL of the dialogue-related stuff. Attach this to an empty GameObject. 
 * This script also reduced my lifespan by 5 years. I owe my life to Unity Documentation, forums, youtube videos, etc.
 */

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    //NOBODY TOLD ME YOU CAN CREATE CUSTOM HEADERS!
    //THIS IS GAME CHANGING
    [Header("UI Elements")]
        [Header("dialogueUI = TextBox")]
        [Header("TextContainer = TextContainer")]
        [Header("ButtonContainer = ButtonContainer")]
        [Header("SpeakerName = CharName")]
        [Header("DialogueBody = Description")]
    
    public GameObject dialogueUI;               //Parent of everything. This is TextBox in the Canvas
    public GameObject textContainer;            //Contains the text
    public GameObject buttonsContainer;         //Contains the button

    public TextMeshProUGUI speakerNameText;     //speakerName is the Character speaking (e.g., You, Narrator, etc)
    public TextMeshProUGUI dialogueBodyText;    //This is the actual dialogue being spoken (e.g., "Hi, I'm the Narrator, and I'm super cool!")

    [Header("Response Buttons")]
        [Header("buttonPrefab = Choice1 Prefab. Don't do Choice2")]
        [Header("Response1Parent = Response1Parent")]
        [Header("Response2Parent = Response2Parent")]
    
    public GameObject buttonPrefab;             
    public Transform response1Parent;           //Response1 appears on the top
    public Transform response2Parent;           //Response2 appears on the bottom

    [Header("Put Conversation Node here and check autoStart")]
    [Header("Please don't make it false!")]
    public Dialogue autoStartConversationNode;
    public bool autoStart = true;

    [Header("Add an audio source here if you want a button to play sound when pressed")]
    [Header("Don't play something too long please")]
    public AudioSource sound;

    [Header("cubeChanImageContainer = ImageContainer")]
    [Header("cubeChanSprite = Image")]
    public Image cubeChanImageContainer;
    public Image cubeChanSprite;


    private bool skipTypewriter; //Only useful when the player wants to skip the typewriter effect

    [Header("Typewriter Speed Settings")]
        [Header("Shorter number = faster speed")]
        [Header("Higher number = slower speed")]
    public float wordSpeed = 0.02f; //You can change it in the inspector if needed. I don't recommend seconds, this is PER character. Not word

    /// <summary>
    /// If Update is called every frame, this is called when the script gets loaded.
    /// "Oh, that's just the Start-" Shut up. No, it's not. Youtube and Google told me so
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
         
        HideDialogue();
    }

   
    private void Update()
    {
        //If the dialogueUI is active AND the player presses Q, skip the super fun typewriting effect (rude)
        if (dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            skipTypewriter = true;
        }
        
    }

    private void Start()
    {
        //Honestly, I don't know why you're given a choice to manually start it.
        //Just always have the auto start checked to true
        if (autoStartConversationNode != null)
        {
            StartDialogue(autoStartConversationNode);
        }

    }

    //This starts the dialogue, what do you want from me
    public void StartDialogue(Dialogue convo)
    {
        StartDialogueNode(convo.rootNode);
    }

    //This is the node that gets called in StartDialogue.
    private void StartDialogueNode(DialogueNode node)
    {
        //Shows the dialogueUI and sets the speakerName to whatever is in the node
        //Remember speakerName = Character name
        //Not changing it, deal with it
        dialogueUI.SetActive(true);
        speakerNameText.text = node.speakerName;

        //If showCubeChan AND cubeChanSprite ARE NOT null, then show cube chan!
        if (node.showCubeChan && node.cubeChanSprite != null)
        {
            cubeChanSprite.sprite = node.cubeChanSprite;
            cubeChanSprite.gameObject.SetActive(true);
        }
        else //otherwise hide her beauty from the world.
        {
            cubeChanSprite.gameObject.SetActive(false);
        }

        //If playSound and sound are not null, play the sound
        //You already seen this
        if (node.playSound && sound != false)
        {
            print("IT WORKS!!!");
            sound.Play();
        }

        StopAllCoroutines();
        
        //See! We did the Coroutine!
        StartCoroutine(TypeWriterEffect(node));
    }

    private IEnumerator TypeWriterEffect(DialogueNode node)
    {
        skipTypewriter = false;
        dialogueBodyText.text = "";

        //Typewriting effeect, this is super important
        //Trust me it is
        //For every character in the dialogueText, it will get affected by the typerwriter effect
        foreach (char c in node.dialogueText)
        {
            //If the player skips the typewriting effect by pressing Q, then it'll instantly display the dialogue 
            //and break
            if (skipTypewriter == true)
            {
                yield return new WaitForSeconds(0.09f); //Small delay to prevent players from spamming Q
                dialogueBodyText.text = node.dialogueText; //whatever the node dialogue is, so is the dialogueBody
                break;
            }

            //We were going to attempt to add a sound effect for each character (like any other game), but
            //we couldn't figure out what sound effect sounded right.

            //Text is plus equal to each character
            dialogueBodyText.text += c;
            yield return new WaitForSeconds(wordSpeed); //All it does is add a delay to each character. You can set the wordSpeed to whatever
        }

       
        skipTypewriter = false;

       //Remember the isBranching from DialogueNode?
       //This is where it gets called
        if (node.isBranching)
        {
            ShowResponses(node);
            yield break;         
        }

        if (node.defaultNextNode != null)
        {
           
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
            StartDialogueNode(node.defaultNextNode);
            yield break;
        }

       //If there are no other nodes after the last node, it'll stay on screen for 
       //6 seconds before hiding. This should never happen, but it's here just in case.
        yield return new WaitForSeconds(6f);
        HideDialogue();
    }


    //This is where I copied the code from the video, stared at it, and wondered what half of it meant
    //If it works, it works.
    private void ShowResponses(DialogueNode node)
    {
       
        //DO NOT SET THIS TO FALSE, IT WILL HIDE THE TEXT WHEN THE RESPONSES APPEAR!
        //THAT IS BAD DESIGN!
        textContainer.SetActive(true);

        //DO NOT SET THIS TO FALSE, IT WILL HIDE THE BUTTONS WHEN CALLED!
        buttonsContainer.SetActive(true);

        foreach (Transform t in response1Parent) Destroy(t.gameObject);
        foreach (Transform t in response2Parent) Destroy(t.gameObject);

        
        if (node.responses.Count > 0)
            CreateResponseButton(node.responses[0], response1Parent, 10);

        if (node.responses.Count > 1)
            CreateResponseButton(node.responses[1], response2Parent, 10);
    }

    
    private void CreateResponseButton(DialogueResponse response, Transform parent, int fontSize)
    {
        //DialogueNode dialogueNode = ScriptableObject.CreateInstance<DialogueNode>();
        //There used to be an if statement that made sure that both sound and playSound are not equal to null
        //Enjoy the free line of code

        var buttonPrefab = Instantiate(this.buttonPrefab, parent);
        var text = buttonPrefab.GetComponentInChildren<TextMeshProUGUI>();
        var button = buttonPrefab.GetComponent<Button>();

        //fontSize changes the Text.fontSize. Don't change the number to anything high. It'll scale off the screen.
        //I wish there was some sort of way to check Auto Size through text.
        text.fontSize = fontSize;
        text.text = response.responseText;

        //Something something unity event functions, I barely understand it
        //At least I used a lambda expression because I used the "this" pointer. 
        //I hate it
        button.onClick.AddListener(() => {
            response.onSelected.Invoke();

            //DON'T SET THIS TO TRUE. KEEP IT FALSE
            buttonsContainer.SetActive(false);

            //Keep this true though. We want text!
            textContainer.SetActive(true);


            //I forgot why I wrote this if the playSoundResponse didn't work as intended
            //Enjoy?
            if (response.sprite != null && cubeChanImageContainer != null)
            {
                //If the response sprite IS NOT null and cubeChanImageContainer's image IS NOT null
                //then cubeChanImageContainer is assigned to the response sprite
                cubeChanImageContainer.sprite = response.sprite;

            }

            //Look at this redudant code that drove me insane until I realized it wouldn't work the way I thought it would
            //Haha, I suffered

            /* if (response.playSoundResponse != false)
            {
                print("why won't you work on me");
                sound.Play();
              
            }
            */

            //This is the scene transition! It... transitions the scene (but in a way that gives headaches BECAUSE IT REFUSED TO WORK)
            if (!string.IsNullOrEmpty(response.loadScene)) //If the string is not null or empty, load the next scene
            {
                SceneManager.LoadScene(response.loadScene);
            }
            else if (response.nextNode != null) //Else, if the next node doesn't have a scene to load, it'll load the next node
            {
                StartDialogueNode(response.nextNode);
            }
            
            
            
        });
    }

    /// <summary>
    /// If the dialogueUI is shown, then it'll return true. Otherwise, it'll return false.
    /// </summary>
    public bool IsDialogueActive()
    {
        return dialogueUI.activeSelf;
    }

    /// <summary>
    /// Hides the dialogueUI when called
    /// </summary>
    private void HideDialogue()
    {
        dialogueUI.SetActive(false);
    }
}

