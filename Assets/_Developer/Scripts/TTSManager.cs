using UnityEngine;

public class TTSManager : MonoBehaviour
{
    public static TTSManager Instance { get; private set; }
    private TextToSpeech tts;

    private void Awake()
    {
        // Ensure only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            tts = gameObject.AddComponent<TextToSpeech>();

#if UNITY_ANDROID && !UNITY_EDITOR
            // Initialize with empty string to warm up TTS engine
            tts.Initialize();
            tts.Speak(""); 
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public methods to use TTS anywhere
    public void Speak(string text)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        tts.Speak(text);
#endif
    }

    public void SetPitch(float pitch)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        tts.SetPitch(pitch);
#endif
    }

    public void SetSpeed(float speed)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        tts.SetSpeed(speed);
#endif
    }

    public void SetLanguage(TextToSpeech.Locale locale)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        tts.SetLanguage(locale);
#endif
    }
}
