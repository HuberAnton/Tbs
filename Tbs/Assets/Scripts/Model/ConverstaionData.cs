using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Refer to your old project for more on this. "El test".
[CreateAssetMenu(fileName = "Conversation_Data", menuName = "Dailogue/New Conversation Data", order = 0)]
public class ConverstaionData : ScriptableObject
{
    // Used to find the conversation.
    // public string Id;

    public List<SpeakerData> list;

    // Pass through all valid 
    public void Load(string[] lines)
    {
        // Create a new Speakerdata
        var data = new SpeakerData1();

        // Cycle through each of the lines.
        for(int i = 0; i < lines.Length; ++i)
        {




        }
    }
}