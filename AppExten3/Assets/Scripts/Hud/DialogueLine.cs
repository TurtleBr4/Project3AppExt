using UnityEngine;
public class DialogueLine
{
    public string characterName;
    public string text;
    public DialogueChoice[] choices; // for branching dialogue
}

public class DialogueChoice
{
    public string choiceText;
    public int nextLineIndex; // the index of the next line to display after the choice
}
