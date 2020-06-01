using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharData = SpriteData.CharData;

public class DialogueSystem : MonoBehaviour
{
    public SpriteData spriteData;

    public float textSpeed = 15f;
    public float size = 1f;
    public Vector2 margins = new Vector2(1, 1);
    public float indentationX = 0;

    public float spaceBetweenChars = 0.1f;
    public float spaceBetweenWords = 0.5f;
    public float spaceBetweenLines = 0.5f;

    private bool conversation = false;
    private ConversationSystem conversationSystem;

    [TextArea(4, 8)]
    public List<string> Dialogue;

    private int index = 0;

    private float timer = 0f;

    [HideInInspector]
    public float posX = 0f;
    [HideInInspector]
    public float posY = 0f;

    private float minX;
    private float maxX;

    private float minY;
    private float maxY;

    private Sprite textBoxSprite;
    private SpriteRenderer spriteRen;

    private int charIndex = 0;

    public struct Word
    {
        public string word;
        public float length;

        public Word(string w, float l)
        {
            word = w;
            length = l;
        }
    }

    private Word currentWord;

    private bool waitValidation = false;
    private bool finishedWriting = false;

    [HideInInspector]
    public bool writingOptions = false;

    //----------------------------------------------------------------------------------------------

    void Awake()
    {
        textBoxSprite = GetComponent<SpriteRenderer>().sprite;
        spriteRen = GetComponent<SpriteRenderer>();

        conversationSystem = GetComponent<ConversationSystem>();

        minX = transform.position.x - spriteRen.size.x / 2 + textBoxSprite.border.x / textBoxSprite.pixelsPerUnit; //border.x = left
        maxX = transform.position.x + spriteRen.size.x / 2 - textBoxSprite.border.z / textBoxSprite.pixelsPerUnit; //border.z = right

        minY = transform.position.y - spriteRen.size.y / 2 + textBoxSprite.border.y / textBoxSprite.pixelsPerUnit; //border.y = bottom
        maxY = transform.position.y + spriteRen.size.y / 2 - textBoxSprite.border.w / textBoxSprite.pixelsPerUnit; //border.w = top

        writingOptions = false;
        //StartWriting(true, false, indentationX, Dialogue);

    }

    //----------------------------------------------------------------------------------------------

    private void fillCurrentWord()
    {
        char[] chars = Dialogue[index].ToCharArray();
        currentWord.word = "";

        int i;
        for(i = charIndex; i < chars.Length && chars[i] != (char)32; i++) // && chars[i] != (char)13 //char(13) is enter
        {
            currentWord.word += chars[i];
            //Debug.Log("i = " + i + ", chars[i] = " + (int) chars[i]);
            if (chars[i] == (char)10)  //char(10) is new line feed
            {
                i++;
                break; 
            }
        }
        currentWord.length = i - charIndex;
    }

    //----------------------------------------------------------------------------------------------

    void Update()
    {
        if (finishedWriting)
        {
            if(conversation)
            {
                conversationSystem.PrintRetorts();
                conversation = false;
            }
            return; //call conversationSystem
        }

        if (waitValidation)
        {
            if(writingOptions)
            {
                waitValidation = false;
                NewLine();
                fillCurrentWord();
                return;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                waitValidation = false;
                ResetTextBox();
                fillCurrentWord();

            }
            return;
        }

        if(CanPrintWord())
        {
            timer += Time.deltaTime * textSpeed;
            if (timer > 1)
            {
                PrintWord();
            }
        }
    }

    //----------------------------------------------------------------------------------------------

    public void StartWriting(bool clearTextBox, bool conversation, float indentX, List<string> dialogue)
    {
        this.conversation = conversation;

        indentationX = indentX;

        this.Dialogue = dialogue;
        index = 0;
        charIndex = 0;

        if (clearTextBox)
        {
            ResetTextBox();
        }
        else
        {
            //NewLine();
        }

        currentWord = new Word("", 0f);
        fillCurrentWord();

        finishedWriting = false;
        waitValidation = false;
    }

    //----------------------------------------------------------------------------------------------

    public void ResetTextBox()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); //delete whats already been written
        }
        posX = minX + margins.x + indentationX;
        posY = maxY - margins.y;
    }

    //----------------------------------------------------------------------------------------------

    public bool CanPrintWord()
    {
        if (CheckEndOfSentence()) return false;
        if (CheckEndOfWord()) return false;

        return true;
    }

    //----------------------------------------------------------------------------------------------

    public void PrintWord()
    {
        posX += spaceBetweenChars;

        char current = Dialogue[index].ToCharArray()[charIndex];
        CharData? attempt = spriteData.GetSprite(current);

        if (CheckNullCharData(attempt, current)) return;

        CharData data = (CharData)attempt;

        if (CheckWordWrapping(data.width)) return;

        Vector3 transform = new Vector3(posX + data.offsetX, posY + data.offsetY, -5f);
        PrintLetter(transform, current, data.sprite);

        timer = 0;
        charIndex++;
        currentWord.length--;
    }

    //----------------------------------------------------------------------------------------------

    public bool CheckEndOfSentence()
    {
        if (index >= Dialogue.Count) //end of sentences
        {
            //then we have finished printing everything
            finishedWriting = true;
            return true;
        }

        if (charIndex >= Dialogue[index].Length)
        {
            index++;
            if (index >= Dialogue.Count) //end of sentences
            {
                //then we have finished printing everything
                finishedWriting = true;
            }
            else //end of sentence
            {
                charIndex = 0;
                waitValidation = true;
            }
            return true;
        }
        return false;
    }

    //----------------------------------------------------------------------------------------------

    public bool CheckEndOfWord()
    {
        if (currentWord.length == 0) //current = ' '
        {
            posX += spaceBetweenWords;
            charIndex++; //increment for the space
            fillCurrentWord();
            return true;
        }
        return false;
    }

    //----------------------------------------------------------------------------------------------

    public bool CheckNullCharData(CharData? attempt, char c)
    {
        if (attempt == null)
        {
            if (c == (char)10) //carriage return
            {
                posY -= spaceBetweenLines;
                posX = minX + margins.x;

                if (posY - margins.y < minY)
                {
                    waitValidation = true;
                }

                currentWord.length -= 1;
                charIndex += 1; //increment by two for carriage return and new line feed

                if (charIndex < Dialogue[index].Length) fillCurrentWord(); //we havent yet reached end of text
            }
            return true;
        }
        return false;
    }

    //----------------------------------------------------------------------------------------------

    public bool CheckWordWrapping(float spriteWidth)
    {
        if (posX + currentWord.length * spaceBetweenWords + currentWord.length * spriteWidth > maxX) //approximate that each char has same width
        {
            NewLine();

            if (writingOptions)
            {
                conversationSystem.sentenceScale[index] += 1;
            }

            if (posY - margins.y < minY) 
            {
                waitValidation = true;
                return true;
            }

        }
        posX += spriteWidth;
        return false;
    }

    //----------------------------------------------------------------------------------------------

    public GameObject PrintLetter(Vector3 transform, char c, Sprite sprite)
    {
        GameObject letter = new GameObject(c.ToString());
        letter.transform.position = transform;
        letter.transform.localScale = new Vector3(size, size, 0);

        letter.AddComponent<SpriteRenderer>();
        letter.GetComponent<SpriteRenderer>().sprite = sprite;
        letter.GetComponent<SpriteRenderer>().sortingLayerID = spriteRen.sortingLayerID;
        letter.GetComponent<SpriteRenderer>().sortingOrder = spriteRen.sortingOrder + 1;
        letter.transform.parent = gameObject.transform;
        return letter;
    }

    //----------------------------------------------------------------------------------------------

    public void NewLine()
    {
        posY -= spaceBetweenLines;
        posX = minX + margins.x + indentationX;
    }

    //----------------------------------------------------------------------------------------------

    private bool GetNextDialogue()
    {
        if(conversation)
        {
           // if (conversationSystem.EndOfConversation(conversationNode)) return false;

            //ConversationNode? node = conversationSystem.GetNextConversationNode(conversationNode);
        }

       /* if (conversationNode.possibilities.Count != 0) //it is empty when there are no retorts
        {
            //call conversationsystem

            
            
        }*/
        return false;
    }


}
