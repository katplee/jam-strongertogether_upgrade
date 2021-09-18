using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("General")]    
    [HideInInspector]
    public GameObject popup;
    private bool press = false;
    [HideInInspector]
    public bool talking=false;    

    [Header("Dialogue")]
    public DialogueTrigger trigger;
    public GameObject instPanel;
    public GameObject spritePanel;

    

    // Start is called before the first frame update
    void Start()
    {
        spritePanel.SetActive(false);
        instPanel.SetActive(false);
        //texto = GameObject.FindGameObjectWithTag("Chat");
        //texting = texto.GetComponent<Text>();        
        //popup = GameObject.FindGameObjectWithTag("PopUp");        
        //PanelCG.alpha = 0;
        //GameDialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Chattype();
    }

    void Chattype()
    {
        if (Input.GetKeyDown(KeyCode.E) && press && !talking)
        {
            //GameDialoguePanel.SetActive(true);
            //LeanTween.moveY(GameDialoguePanel, GameDialoguePanel.transform.position.y + 0.4f, 0.3f).setEase(LeanTweenType.easeInSine);
            //LeanTween.alphaCanvas(PanelCG, 1f, 0.6f);
            talking = true;
            trigger.NPCTriggerDialogue(this);
            spritePanel.SetActive(false);
            instPanel.SetActive(false);            
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.CompareTag("Player"))
        {            
            spritePanel.SetActive(true);
            instPanel.SetActive(true);
            press = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spritePanel.SetActive(false);
            instPanel.SetActive(false);
            press = false;
            talking = false;
        }        
    }  

    public void resetTalk()
    {
        talking = false;
        spritePanel.SetActive(true);
        instPanel.SetActive(true);
    }
}
