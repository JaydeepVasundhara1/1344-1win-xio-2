using UnityEngine;
using UnityEngine.UI;  // For regular UI Text
using TMPro;          // For TextMeshPro (optional)

public class WorkoutTimerManager : MonoBehaviour
{
    public static float timer = 0f;  // Time in seconds
    public static bool timerRunning = false;

    private int pressCount;
    private float lastPressTime;

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }

         if (Input.GetKeyDown(KeyCode.Escape))
            {
                RegisterPress();
            }

        
    }

    private void RegisterPress()
    {
        float currentTime = Time.unscaledTime;

        if (currentTime - lastPressTime > 1f)
        {
            pressCount = 0;
        }

        pressCount++;
        lastPressTime = currentTime;

        if (pressCount >= 2)
        {
            QuitApplication();
        }
    }

    private static void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // Start the timer with a specific time in seconds
    public static void StartTimer()
    {
        timerRunning = true;
    }

    // Stop the timer
    public static void StopTimer()
    {
        timerRunning = false;
    }

    // Reset the timer
    public static void ResetTimer()
    {
        timer = 0f;
        timerRunning = false;
    }
}