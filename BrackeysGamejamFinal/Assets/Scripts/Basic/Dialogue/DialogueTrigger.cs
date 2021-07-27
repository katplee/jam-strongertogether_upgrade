using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueText gameText;
    public Text Title;
    public Text Body;

    public void NPCTriggerDialogue(NPC npc)
    {
        DialogueManager.instance.StartDialogue(gameText, Title, Body, npc);
    }
}
