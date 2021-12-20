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
    public List<SpeakerData> list;
}