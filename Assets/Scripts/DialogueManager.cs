using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    //If you ask me, I don't know myself. I watched "How to Create a Dialogue System With Choices In Unity | Unity Game Dev Tutorial"
    //And copied the code shown on video.

    public static DialogueManager Instance { get; private set; }

    //NOBODY TOLD ME YOU CAN CREATE CUSTOM HEADERS!
    //THIS IS GAME CHANGING
    [Header("UI Elements")]
        [Header("dialogueUI = TextBox")]
        [Header("TextContainer = TextContainer")]
        [Header("ButtonContainer = ButtonContainer")]
        [Header("SpeakerName = CharName")]
        [Header("DialogueBody = Description")]
    
    public GameObject dialogueUI;               // Parent of everything. This is TextBox in the Canvas
    public GameObject textContainer;            // the textbox panel
    public GameObject buttonsContainer;         // the two-button panel

    public TextMeshProUGUI speakerNameText;     //speakerName is the Character speaking (e.g., You, Narrator, etc)
    public TextMeshProUGUI dialogueBodyText;    //This is the actual dialogue being spoken (e.g., "Hi, I'm the Narrator, and I'm super cool!")

    [Header("Response Buttons")]
        [Header("buttonPrefab = Choice1 Prefab. Don't do Choice2")]
        [Header("Response1Parent = Response1Parent")]
        [Header("Response2Parent = Response2Parent")]
    
    public GameObject buttonPrefab;             // just one prefab
    public Transform response1Parent;           // Appears on the top
    public Transform response2Parent;           // Appears on the bottom

    [Header("Put Conversation Node here and check autoStart")]
    [Header("Please don't make it false!")]
    public Dialogue autoStartConversationNode;
    public bool autoStart = true;


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
        if (autoStartConversationNode != null)
        {
            StartDialogue(autoStartConversationNode);
        }

    }
    public void StartDialogue(Dialogue convo)
    {
        StartDialogueNode(convo.rootNode);
    }

    private void StartDialogueNode(DialogueNode node)
    {
        //Shows the dialogueUI and sets the speakerName to whatever is in the node
        //Remember speakerName = Character name
        //Not changing it, deal with it
        dialogueUI.SetActive(true);
        speakerNameText.text = node.speakerName;


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

            //play a sound here or something idk
            dialogueBodyText.text += c;
            yield return new WaitForSeconds(wordSpeed); //All it does is add a delay to each character. You can set the wordSpeed to whatever
        }

       
        skipTypewriter = false;

       //Remember the isBranching from DialogueNode?
       //This is where it gets called
        if (node.isBranching)
        {
            ShowResponses(node);
            yield break;          // stop here until a button is clicked
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



    private void ShowResponses(DialogueNode node)
    {
       
        //DO NOT SET THIS TO FALSE, IT WILL HIDE THE TEXT WHEN THE RESPONSES APPEAR!
        //THAT IS BAD DESIGN!
        textContainer.SetActive(true);

        //DO NOT SET THIS TO FALSE, IT WILL HIDE THE BUTTONS WHEN CALLED!
        buttonsContainer.SetActive(true);

        // clear old buttons
        foreach (Transform t in response1Parent) Destroy(t.gameObject);
        foreach (Transform t in response2Parent) Destroy(t.gameObject);

        // only two possible responses—ignore extras
        if (node.responses.Count > 0)
            CreateResponseButton(node.responses[0], response1Parent, 10);

        if (node.responses.Count > 1)
            CreateResponseButton(node.responses[1], response2Parent, 10);
    }

    

    private void CreateResponseButton(DialogueResponse response, Transform parent, int fontSize)
    {
        var buttonPrefab = Instantiate(this.buttonPrefab, parent);
        var text = buttonPrefab.GetComponentInChildren<TextMeshProUGUI>();
        var button = buttonPrefab.GetComponent<Button>();

        //fontSize changes the Text.fontSize. Don't change the number to anything high. It'll scale off the screen.
        //I wish there was some sort of way to check Auto Size through text.
        text.fontSize = fontSize;
        text.text = response.responseText;
        button.onClick.AddListener(() =>
        {
            // invoke designer-assigned effects
            response.onSelected.Invoke();

            // back to text mode
            //DON'T SET THIS TO TRUE. KEEP IT FALSE
            buttonsContainer.SetActive(false);

            //Keep this true though. We want text!
            textContainer.SetActive(true);

            //scene transitoon
            if (!string.IsNullOrEmpty(response.loadScene))
            {
                SceneManager.LoadScene(response.loadScene);
            }
            else if (response.nextNode != null)
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

