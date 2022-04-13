using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Old version
[System.Serializable]
public class SpeakerData
{
    public List<string> messages;
    public Sprite speaker;
    public TextAnchor anchor;
}


// Data class of ConversationData.
// Think of each individual one of these as a seperte setup.
// Cycling to the next will result in a 'reset' of the canvas.
// Eg 2 characters appear on screen and talk back and forth between each other.
// That would all be contatined in one of these.

// If you would like to add or remove people in the conversation you would need a new Speakerdata
[System.Serializable]
public class SpeakerData1
{
    // All speakers in conversation
    public Sprite speaker1Left;
    public Sprite speaker1Right;
    public Sprite speaker2Left;
    public Sprite speaker2Right;

    // Current speaker
    public string[] speakerName;

    // If speaker is on the left/right
    // May need to be changed
    public TextAnchor[] speakerDirection;

    // All lines for this current setup.
    // If a change is needed new speaker data is created.
    public string[] lines;

    // No sound system yet
    //public Sound sound;
}


