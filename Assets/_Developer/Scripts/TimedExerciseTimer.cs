using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimedExerciseTimer : MonoBehaviour
{
    public float totalTime;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= totalTime)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay();
        }           
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        // Format the time as MM:SS
        GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
