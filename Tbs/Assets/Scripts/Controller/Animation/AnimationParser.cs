using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;


// Handles the creation of animation controllers for units.
// Ideally you right clock on a folder
public static class AnimationParser
{
    [MenuItem("Assets/Parse Animation")]
    public static void Parse()
    {
        List<string> folders = ValidateSelection();

        CreateDirectories();
        ParseSelectedFolders(folders);


        //
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    static void CreateDirectories()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Animations"))
            AssetDatabase.CreateFolder("Assets/Resources", "Animations");
    }

    // Checks if what is selected is a folder with animation clips.
    static List<string> ValidateSelection()
    {
        List<string> validFolders = new List<string>();
        foreach (var obj in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets))
        {
            // Get the path of the selected.
            var path = AssetDatabase.GetAssetPath(obj);

            // If nothing selected null or empty
            if (string.IsNullOrEmpty(path))
                continue;

            // Checks if the file path exists
            // Proving that what is selected is a folder
            if (System.IO.Directory.Exists(path))
            {
                validFolders.Add(path);
            }
            // This should return the files directory.
            // Eg call this on an animation instead of the folder.
            else if (System.IO.File.Exists(path))
            {
                validFolders.Add(System.IO.Path.GetDirectoryName(path));
            }
        }
        return validFolders;
    }

    // Cycle through given folders to see if an animation is present.
    // If is present will create an animation controller of the folder name
    // and attach any clips found in to the folder with states being there
    // names.
    static void ParseSelectedFolders(List<string> folders)
    {


        // In order to create this asset correctly you need the fullfilename and filetype.
        var animController = AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/Animations/Test.controller");
        // This will not change the name of the controller.
        animController.name = "Empty";

        // Cycle through each folder
        for (int i = 0; i < folders.Count; ++i)
        {
            // Get all files
            string[] files = Directory.GetFiles(folders[i]);

            // Cycle through each file in folder i.
            for (int j = 0; j < files.Length; ++j)
            {
                // Check if file is an .fbx
                string[] elements = files[j].Split('.');
                // Consider a switch here for different file types.
                if (elements[elements.Length - 1] == "fbx")
                {
                    // if valid file type get the current file get clip and add to animator
                    var anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(files[j]);
                    if (anim != null)
                    {
                        // Needs to be correctly named.
                        Debug.Log(anim.name);

                        animController.AddMotion(anim);
                    }
                }
            }
        }
    }
}

