using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
 
[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}
 
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
 
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
 
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject hint;

    private bool _isInZone = false;
    public void TriggerDialogue()
    {
        DialogueManager.Instance.dialoguePanel.SetActive(true);
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _isInZone = true;
            hint.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _isInZone = false;
            hint.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && _isInZone)
        {
            TriggerDialogue();
        }
    }
    
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if(collision.tag == "Player" && Input.GetKey(KeyCode.E))
    //     {
    //         TriggerDialogue();
    //     }
    // }
}