using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make singleton class
public class ConversationManagerSystem : MonoBehaviour
{
    [HideInInspector]
    public static Dictionary<string, ConversationNode> conversations;

    public TextAsset jsonFile;

    void Awake()
    {
        conversations = Conversations.CreateFromJSON(jsonFile.text);
        //ConversationSystem.options = new string[conversations.Keys.Count];
        //conversations.Keys.CopyTo(ConversationSystem.options, 0);
        DontDestroyOnLoad(gameObject);
    }

    public ConversationNode GetConversation(string id)
    {
        if (!conversations.ContainsKey(id)) Debug.Log("couldn't find conversation with id: " + id);
        return conversations[id];
    }

    
}
