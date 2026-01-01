using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{
    public string folderName;  // Folder name from where to load the sprites
    public float frameRate = 0.1f; // Time interval between frames (in seconds)

    private Sprite[] sprites;  // Array to hold the loaded sprites
    private Image uiImage;  // Reference to the UI Image component

    private bool spritesLoaded;

    void Start()
    {
        // Get the Image component attached to the GameObject
        uiImage = GetComponent<Image>();

        // Load sprites from the folder
        LoadSprites();

        // Start the sprite animation coroutine
        if (sprites.Length > 0)
        {
            StartCoroutine(AnimateSprites());
        }
    }

    private void OnEnable()
    {
        if (spritesLoaded)
        {
            StartCoroutine(AnimateSprites());
        }
    }

    // Method to load sprites from the folder
    void LoadSprites()
    {
        // Load sprites from the specified folder in Resources
        sprites = Resources.LoadAll<Sprite>($"assets/{folderName}");

        // Check if the folder has any sprites
        if (sprites.Length == 0)
        {
            Debug.LogError("No sprites found in the folder: " + folderName);
        }

        else
        {
            spritesLoaded = true;
        }
    }

    // Coroutine to cycle through the sprites at the specified interval
    IEnumerator AnimateSprites()
    {
        int index = 0;

        while (true)
        {
            // Set the sprite of the UI Image to the current sprite in the array
            uiImage.sprite = sprites[index];

            // Wait for the specified frame rate before switching to the next sprite
            yield return new WaitForSeconds(frameRate);

            // Move to the next sprite in the array, and loop back to the start if needed
            index = (index + 1) % sprites.Length;
        }
    }
}
