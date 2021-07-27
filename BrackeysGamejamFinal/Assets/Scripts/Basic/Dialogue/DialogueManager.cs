using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("General")]
    private Queue<string> sentences;
    public static DialogueManager instance;
    [HideInInspector]
    public int maxIndex;
    [HideInInspector]
    public int currentIndex = 0;
    private bool hasStarted = false, isDisplayingText = false;

    [Header("Game Controls")]    
    public NPC npc;

    [Header("UI")]
    public Text primaryTitle;
    public Text primaryText;
    public GameObject mainPanel;

    void Awake() //Singleton pattern!
    {
        if (instance != null)
        {
            Debug.LogError("More than one DialogueManager in the scene");
            return;
        }
        instance = this;
    }

    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (hasStarted && Input.GetKey(KeyCode.Space) && !isDisplayingText)
        {
            DisplayNextSentence();
        }
    }

    //For text tutorial in the beginning
    public void StartDialogue(DialogueText dialogue, Text Title, Text Body, NPC _npc)
    {
        primaryTitle = Title;
        primaryText = Body;
        npc = _npc;
        sentences.Clear();
        hasStarted = true;
        mainPanel.SetActive(true);

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        if (primaryTitle != null)
            primaryTitle.text = dialogue.Title;
        maxIndex = sentences.Count;
        currentIndex = 0;

        // this to avoid double SOUNDS
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        currentIndex++;
        string s = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(s));
    }

    public void DisplayNextSentence()
    {
        isDisplayingText = true;
        //AudioManager.instance.Play("ButtonPress");
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        currentIndex++;
        string s = sentences.Dequeue();        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(s));
    }

    IEnumerator TypeSentence(string sentence)
    {
        primaryText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            primaryText.text += letter;
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        isDisplayingText = false;
    }

    public void EndDialogue()
    {
        hasStarted = false;
        mainPanel.SetActive(false);
        npc.resetTalk();
    }
}
