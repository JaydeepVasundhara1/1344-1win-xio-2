using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutSoundManager : MonoBehaviour
{
    public static bool isSoundOn;

    // Start is called before the first frame update
    void Start()
    {
        isSoundOn = PlayerPrefs.GetString("Sound", "ON") == "ON" ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ChangeSoundStatus()
    {
        isSoundOn = !isSoundOn;

        string soundStatus = isSoundOn == true ? "ON" : "OFF";

        PlayerPrefs.SetString("Sound", soundStatus);
    }
}
