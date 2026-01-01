#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ColorApplicator))]
public class ColorApplicatorEditor : Editor
{
    private SerializedProperty _imagesProperty;
    private SerializedProperty _textsProperty;
    private SerializedProperty _imageColorProperty;
    private SerializedProperty _textColorProperty;

    private void OnEnable()
    {
        _imagesProperty = serializedObject.FindProperty("_images");
        _textsProperty = serializedObject.FindProperty("_texts");
        _imageColorProperty = serializedObject.FindProperty("_imageColor");
        _textColorProperty = serializedObject.FindProperty("_textColor");
    }

    public override void OnInspectorGUI()
    {
        // Draw the default properties
        DrawDefaultInspector();

        EditorGUILayout.Space(20);

        // Create the apply button
        ColorApplicator applicator = (ColorApplicator)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // Apply Colors Button
        if (GUILayout.Button("Apply Colors", GUILayout.Width(200), GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Apply Colors",
                "Are you sure you want to apply the selected colors to all images and texts?",
                "Apply", "Cancel"))
            {
                // Record undo for the entire operation
                Undo.RecordObject(applicator, "Apply Colors to All Elements");
                
                // Apply colors to all images and texts
                applicator.ApplyColorsInEditor();
                
                // Mark the scene as dirty to save changes
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                
                Debug.Log("Colors applied successfully to " + 
                         applicator.Images_dark.Count + " images and " + 
                         applicator.Texts.Count + " text elements.");
            }
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        // Color Preview Section
        EditorGUILayout.LabelField("Color Preview", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginVertical(GUI.skin.box);
        
        // Dark Image color preview
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Image Dark :", GUILayout.Width(80));
        Rect imageRect = GUILayoutUtility.GetRect(50, 20);
        EditorGUI.DrawRect(imageRect, applicator.ImageColor_dark);
        EditorGUILayout.EndHorizontal();

          //Light Image color preview
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Image Light:", GUILayout.Width(80));
        Rect imageRect_light = GUILayoutUtility.GetRect(50, 20);
        EditorGUI.DrawRect(imageRect_light, applicator.ImageColor_light);
        EditorGUILayout.EndHorizontal();
        
        // Text color preview
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Text Color:", GUILayout.Width(80));
        Rect textRect = GUILayoutUtility.GetRect(50, 20);
        EditorGUI.DrawRect(textRect, applicator.TextColor);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
#endif