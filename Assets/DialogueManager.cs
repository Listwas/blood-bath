using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public Text characterName;
    public Text dialogueArea;
    public GameObject panelTop;
    public GameObject panelBottom;
    private bool doneTyping = false;

    private Queue<DialogueLine> lines;

    public float typingSpeed = 0.2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
        Active(false);
    }

    // void matchdialogue{
    // zmiana w dikt
    // try match with dikt
    // if matched startdialogue
    // }

    public void StartDialogue(Dialogue dialogue)
    {
        Active(true);

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines) {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0) {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        doneTyping = false;
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray()) {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        doneTyping = true;
    }
    
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && doneTyping) {
            DisplayNextDialogueLine();
        }
    }

    void EndDialogue() { Active(false); }

    void Active(bool state)
    {
        characterIcon.gameObject.SetActive(state);
        characterName.gameObject.SetActive(state);
        dialogueArea.gameObject.SetActive(state);
        panelTop.SetActive(state);
        panelBottom.SetActive(state);
    }
}