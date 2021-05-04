using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Button))]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Button myButton = target as Button;
        if (myButton.haveConfirm)
        {
            
        }
    }
}