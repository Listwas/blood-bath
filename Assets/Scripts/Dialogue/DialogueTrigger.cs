using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public string conditionName;
    
    public bool isThisDialogueCondition;//czy ten dialog sam w sobie jest condition
    public string whatNewCondition;
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

 
public class DialogueTrigger : MonoBehaviour
{
    
    public List<Dialogue> availableDialogues;//bedzie miec w sobie dialogi ktore w danej chwili moze powiedziec npc
    public List<Dialogue> allDialogues;// bedzie miec wszystkie dialogi danego npc

    public List<string> allConditionsForThisNPC;

    [SerializeField] private string currentCondition;


//subskrypcja do eventu
    private void OnEnable()
    {
        DialogueManager.OnConditionDialogue += MyCondition;
    }

    private void OnDisable()
    {
        DialogueManager.OnConditionDialogue -= MyCondition;
    }

    public void TriggerDialogue()
    {
        Dialogue selectedDialogue = availableDialogues[UnityEngine.Random.Range(0, availableDialogues.Count)];

        
        DialogueManager.Instance.StartDialogue(selectedDialogue);
        if(selectedDialogue.isThisDialogueCondition ==true){
            DialogueManager.DialogueEvent(selectedDialogue.whatNewCondition);
        }
    }
 
    private void OnTriggerEnter(Collider collision)
    {
        // jak wedziemy w trigger to wł ikonke nad npc
        if (collision is BoxCollider)
        //and if f 
            {TriggerDialogue();}
            
        
    }

    //reset listy dostepnych dialogow
     public void ResetCurrentDialogs()
    {
        availableDialogues.Clear();
    }

//sprawdza czy ten warunek dotyczy tego npc
    private void MyCondition(string newCondition){
        int i = 0;
        foreach (string condition in allConditionsForThisNPC)
        {
           if(allConditionsForThisNPC[i] == newCondition){
                currentCondition = newCondition;
                ChangeAvilableDialogueList();
            }
            else{
                Debug.Log("warunek nie dotyczy mnie");
                i+= 1;
            }
  
        }
       }
    

    private void ChangeAvilableDialogueList(){
        ResetCurrentDialogs();
        foreach (Dialogue dial in allDialogues){
            AddToCurrentDialogs(dial);
        };
        
    }
    public void AddToCurrentDialogs(Dialogue thisDialogue)
        {
            Debug.Log("Dodanie do dialogow");
            if(thisDialogue.conditionName == currentCondition){
                availableDialogues.Add(thisDialogue);
            }
            
        }

}