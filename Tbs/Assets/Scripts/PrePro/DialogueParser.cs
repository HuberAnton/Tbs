using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;


// Pre produciton script.
// Creates dialogue assets from a csv located at the location
// given in the settings. 
public class DialogueParser
{

    // Values that tell the parser what each collumn does.

    // When loading the level will find the file of the language
    // and load the assets.

    public static readonly int LevelId = 0;

    // Used by calls from event system to load the correct dialogue asset

    static readonly int DialogueId = 1;

    // A 2D image of speaker

    public static readonly int Speaker1Left = 2;
    public static readonly int Speaker1Right = 3;
    public static readonly int Speaker2LeftBack = 4;
    public static readonly int Speaker2RightBack = 5;

    // Name

    public static readonly int SpeakerName = 6;

    // Allignment
    // Left or right speaker

    public static readonly int SpeakerDireciton = 7;

    // Words

    public static readonly int Line = 8;

    // Sound
    // Plays at start of line.

    //static readonly int Sound = 8;


    [MenuItem("Pre Production/Parse Conversations")]
    public static void Parse()
    {
        // Create folder for level dailogue
        // Read data in file
        // Get and overwrite or create file at location

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }



    static void ParseConversation()
    {
        string filePath = Application.dataPath + "/Settings/Conversations.csv";
        if (!File.Exists(filePath))
        {
            Debug.LogError("Can not find file of conversation data. Should be located in setings/conversations.csv");
            return;
        }

        string[] readText = File.ReadAllLines("Assets/Settings/Conversations.csv");
        filePath = "Assets/Settings/Resources/";


        // if only 1 row then only the titles exist
        if (readText.Length > 1)
            // First row is just names so skip.
            for (int i = 1; i < readText.Length; ++i)
            {
                // Need to check the first collumn.
                // If first collumn has something we create a level converstation data.
                // Then


            }
    }




    //static ConverstaionData GetOrCreate(string dialogueName)
    //{
    //    string fullPath = string.Format("Assets/Resources/Jobs/{0}.asset", dialogueName);

    //    var data = AssetDatabase.LoadAssetAtPath<ConverstaionData>(fullPath);

    //    if (data == null)
    //        data = Create(fullPath);

    //    return data;
    //}

    //static ConverstaionData Create(string fullPath)
    //{
    //    ConverstaionData data = new ConverstaionData();

    //    AssetDatabase

    //    return ;
    //}


}