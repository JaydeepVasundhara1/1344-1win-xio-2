using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExerciseTitleData : MonoBehaviour
{
    public Image _image;
    public TMP_Text _title;
    public TMP_Text _time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetExerciseTitleData(string title, string time, string timeType)
    {
        _title.text = title;

        if(timeType == "step")
        {
            _time.text = "X " + time;
        }
        else
        {
            _time.text = time;
        }

        string titleWithoutSpaces = title.Replace(" ", "").Replace("-", "").Replace("&", "");
        _image.GetComponent<SpriteAnimator>().folderName = titleWithoutSpaces;
    }
}
