using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CustomMazeGenerator))]
public class GeneratorEditor : Editor
{
  public override void OnInspectorGUI()
  {
    CustomMazeGenerator myTarget = (CustomMazeGenerator)target;
    base.OnInspectorGUI();

    if (GUILayout.Button("Generate maze"))
    {
      myTarget.GenerateMazes();
    }
  }
}