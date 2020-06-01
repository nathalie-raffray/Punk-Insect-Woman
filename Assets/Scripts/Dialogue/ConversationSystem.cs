using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class ConversationSystem : MonoBehaviour
{
    public bool showSpeakerImage = false;

    private DialogueSystem dialogueSystem;
    private int optionIndex = 0;

    public Sprite selectorSprite;
    private GameObject selector  ;

    private ConversationNode currentConversation;

    private bool CanMoveSelector = false;

    public string conversationName;

    [HideInInspector]
    public List<int> sentenceScale;

    //----------------------------------------------------------------------------------------------

    void Start()
    {
        sentenceScale = new List<int>();
        dialogueSystem = GetComponent<DialogueSystem>();
        //assert dialogue system. conversation system cannot exist without dialogue system.
        currentConversation = ConversationManagerSystem.conversations[conversationName];
        bool isConversation = currentConversation.answers.Count > 0 ? true : false;
        dialogueSystem.StartWriting(true, isConversation, 0, currentConversation.retort);
    }

    //----------------------------------------------------------------------------------------------

    void Update()
    {
        if (CanMoveSelector)
        {
            if (Input.GetKeyDown("up"))
            {
                if(optionIndex > 0)
                {
                    optionIndex--;
                    selector.transform.position += new Vector3(0, dialogueSystem.spaceBetweenLines * sentenceScale[optionIndex], 0);
                }
            }
            else if(Input.GetKeyDown("down"))
            {
                if(optionIndex < currentConversation.answers.Count-1)
                {
                    optionIndex++;
                    selector.transform.position -= new Vector3(0, dialogueSystem.spaceBetweenLines * sentenceScale[optionIndex-1], 0);
                }
            }
            else if(Input.GetKeyDown("space"))
            {
                dialogueSystem.writingOptions = false;
                currentConversation = currentConversation.answers[optionIndex].next;
                bool isConversation = currentConversation.answers.Count > 0 ? true : false;
                dialogueSystem.StartWriting(true, isConversation, 0, currentConversation.retort);
                optionIndex = 0;
                CanMoveSelector = false; 
            }
        }
    }

    //----------------------------------------------------------------------------------------------

    public bool EndOfConversation()
    {
        if (currentConversation.answers.Count == 0 || optionIndex >= currentConversation.answers.Count) return true;
        return false;
    }

    //----------------------------------------------------------------------------------------------

    public void PrintRetorts()
    {
        //print selector
        dialogueSystem.NewLine();
        Vector3 transform = new Vector3(dialogueSystem.posX, dialogueSystem.posY, -5f);

        selector = dialogueSystem.PrintLetter(transform, '>', selectorSprite);
        optionIndex = 0;

        //print each option
        float indentX = selectorSprite.bounds.size.x + dialogueSystem.spaceBetweenChars;

        List<string> dialogue = new List<string>();
        foreach (Answer answer in currentConversation.answers)
        {
            dialogue.Add(answer.option);
        }
        sentenceScale.Clear();
        for(int i = 0; i < dialogue.Count; i++)
        {
            sentenceScale.Add(1);
        }
        dialogueSystem.writingOptions = true;

        dialogueSystem.StartWriting(false, false, indentX, dialogue);

        CanMoveSelector = true;
    }

    //----------------------------------------------------------------------------------------------

}
