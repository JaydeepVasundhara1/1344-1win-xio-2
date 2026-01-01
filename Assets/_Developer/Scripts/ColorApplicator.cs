using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ColorApplicator : MonoBehaviour
{
    public static ColorApplicator instance;

    [Header("Dark Images Settings")]
    [SerializeField] private List<Image> _images_dark = new List<Image>();
    [SerializeField] private Color _imageColor_dark = Color.white;


    [Header("Light Images Settings")]
    [SerializeField] private List<Image> _images_light = new List<Image>();
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Color _imageColor_light = Color.white;

    [Header("Text Settings")]
    [SerializeField] private List<TMP_Text> _texts = new List<TMP_Text>();
    [SerializeField] private Color _textColor = Color.white;

    // Public properties for editor script access
    public List<Image> Images_dark => _images_dark;
    public List<Image> Images_light => _images_light;
    public List<TMP_Text> Texts => _texts;
    public Color ImageColor_dark => _imageColor_dark;
    public Color ImageColor_light => _imageColor_light;
    public Color TextColor => _textColor;


    private void Awake()
    {
        instance = this;
    }
#if UNITY_EDITOR
    // This method is called from the editor script
    public void ApplyColorsInEditor()
    {
        ApplyImageColors_dark();
        ApplyImageColors_light();
        ApplyTextColors();
    }

    private void ApplyImageColors_dark()
    {
        if (_images_dark == null) return;

        Undo.RecordObjects(_images_dark.ToArray(), "Apply Image Colors Dark");
        foreach (var image in _images_dark)
        {
            if (image != null)
            {
                image.color = _imageColor_dark;
                EditorUtility.SetDirty(image);
            }
        }
    }
 private void ApplyImageColors_light()
    {
        if (_images_light == null) return;

        Undo.RecordObjects(_images_light.ToArray(), "Apply Image Colors Light");
        foreach (var image in _images_light)
        {
            if (image != null)
            {
                image.color = _imageColor_light;
                EditorUtility.SetDirty(image);
            }
        }
        mainCamera.backgroundColor = _imageColor_light;
    }

    private void ApplyTextColors()
    {
        if (_texts == null) return;

        Undo.RecordObjects(_texts.ToArray(), "Apply Text Colors");
        foreach (var text in _texts)
        {
            if (text != null)
            {
                text.color = _textColor;
                EditorUtility.SetDirty(text);
            }
        }
    }

    // For internal use only - called when values change in inspector
    private void OnValidate()
    {
        // This prevents runtime execution but allows editor updates
        if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // You can optionally auto-apply here, but the button gives more control
        }
    }
#endif
}