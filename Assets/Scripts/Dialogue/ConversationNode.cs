using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Conversations
{
    public List<ConversationStart> convos;

    public static Dictionary<string, ConversationNode> CreateFromJSON(string jsonString)
    {
        Conversations c = JsonUtility.FromJson<Conversations>(jsonString);
        Dictionary<string, ConversationNode> dict = new Dictionary<string, ConversationNode>();

        foreach(ConversationStart convoStart in c.convos)
        {
            dict[convoStart.conversationId] = convoStart.start;
        }
        return dict;
    }
}

[System.Serializable]
public class ConversationStart
{
    public string conversationId;

    public ConversationNode start;
}

[System.Serializable]
public class ConversationNode 
{
    public string speaker; 

    public List<string> retort;

    public List<Answer> answers;

    public ConversationNode()
    {
        speaker = "INVALID";
        answers = new List<Answer>(); //necessary?
        retort = new List<string>();
    }
}

[System.Serializable]
public class Answer
{
    public string option;

    public ConversationNode next; //WARNING: to make this non recursive make it an int id. 
}
