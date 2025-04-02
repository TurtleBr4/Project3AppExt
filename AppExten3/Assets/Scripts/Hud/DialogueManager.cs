using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;  // Text UI element to show dialogue
    public GameObject dialogueBox; // Panel to enable/disable dialogue UI
    public bool isYapping = false;

    private string[] currentDialogue;
    private int currentLineIndex = 0;

    public void startDialogue(NPCInteraction npc)
    {
        isYapping = true;
        currentDialogue = npc.thingsToSay;
        currentLineIndex = 0;
        dialogueBox.SetActive(true);
        showDialogueLine();
    }

    void showDialogueLine()
    {
        if (currentLineIndex < currentDialogue.Length)
        {
            dialogueText.text = currentDialogue[currentLineIndex];
            currentLineIndex++;
        }
        else
        {
            endDialogue();
        }
    }

    public void onNextLine()
    {
        showDialogueLine();
    }

    void endDialogue()
    {
        isYapping = false;
        dialogueBox.SetActive(false);
        currentLineIndex = 0;
    }
}
