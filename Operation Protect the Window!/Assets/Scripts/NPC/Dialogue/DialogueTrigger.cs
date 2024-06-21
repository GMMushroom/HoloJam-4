using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region Data Classes

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public string nextDialogueTag;
}

[System.Serializable]
public class Dialogue
{
    public DialogueCharacter dialogueCharacter;
    public string dialogueTag;
    [TextArea(3, 10)]
    public string dialogueText;
    public DialogueOption[] options;
    public string nextDialogueTag;
}

#endregion

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button[] optionButtons;
    public Button continueButton;

    private Dialogue _currentDialogue;
    
    public GameObject hint;

    private bool _isInZone = false;

    private void Start()
    {
        _currentDialogue = dialogues.Length > 0 ? dialogues[0] : null;
        dialoguePanel.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isInZone)
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (_currentDialogue != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = _currentDialogue.dialogueText;

            int optionCnt = _currentDialogue.options.Length;
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < optionCnt)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentDialogue.options[i].optionText;
                    int index = i;
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => SelectOption(index));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
            
            continueButton.gameObject.SetActive(optionCnt == 0);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ContinueDialogue);
        }
    }

    public void SelectOption(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex <= _currentDialogue.options.Length)
        {
            string nextTag = _currentDialogue.options[optionIndex].nextDialogueTag;
            _currentDialogue = System.Array.Find(dialogues, d => d.dialogueTag == nextTag);
            TriggerDialogue();
        }
    }

    public void ContinueDialogue()
    {
        if (string.IsNullOrEmpty(_currentDialogue.nextDialogueTag))
        {
            EndDialogue();
        }
        else
        {
            _currentDialogue = System.Array.Find(dialogues, d => d.dialogueTag == _currentDialogue.nextDialogueTag);
            TriggerDialogue();
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    #region Collider Triggers

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

    #endregion
}