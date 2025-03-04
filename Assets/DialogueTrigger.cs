using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;

    public string condition;
    // char sound
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
    // public string dialogueName;
    // dialog condition?

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

[System.Serializable]
public class Conditions
{
    public string conditionName;
    public bool conditionBool;
}

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> availableDialogues;
    public List<Conditions> availableConditions;

    public void TriggerDialogue()
    {
        Dialogue selectedDialogue =
          availableDialogues[UnityEngine.Random.Range(0, availableDialogues.Count)];

        DialogueManager.Instance.StartDialogue(selectedDialogue);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // jak wedziemy w trigger to wł ikonke nad npc
        if (collision is BoxCollider)
        // and if f
        {
            TriggerDialogue();
        }
    }
}