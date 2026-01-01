using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartWorkoutData : MonoBehaviour
{
    public Image _image;
    public TMP_Text _time;
    public TMP_Text _title;
    public Button infoBtn;
    public Button soundButton;
    public Sprite soundSprite;
    public Sprite muteSprite;
    public Button backBtn;
    public Button previousBtn;
    public Button nextBtn;
    public Button workoutCompleteBtn;
    public GameObject workoutScroller;
    public GameObject workoutTimer;
    public TMP_Text timedExerciseTimerText;
    public Image workoutTimerFill;
    public TMP_Text timerText;
    public GameObject progressTrackerParent;
    public GameObject progressTrackerBarPrefab;
    public Sprite progressTrackerBarCompleteSprite;

    private float totalTimer;
    private bool isTimedWorkout;
    private int workoutTime;
    private float workoutTimeCounter;
    private bool isWorkoutTimeCounterRunning;
    private string titleTTS;
    private bool dataSet;

    // Start is called before the first frame update
    void Start()
    {
        backBtn.GetComponent<Image>().color = ColorApplicator.instance.TextColor;
        _time.color = ColorApplicator.instance.TextColor;
        timerText.color = ColorApplicator.instance.TextColor;
        workoutTimerFill.GetComponent<Image>().color = ColorApplicator.instance.ImageColor_dark;
    }

    private void OnEnable()
    {
        if (dataSet)
        {
            if (isTimedWorkout)
            {
                isWorkoutTimeCounterRunning = true;
            }

            if (WorkoutSoundManager.isSoundOn)
            {
                TTSManager.Instance.Speak(titleTTS);
                TTSManager.Instance.SetSpeed(0.5f);
                TTSManager.Instance.SetLanguage(TextToSpeech.Locale.US);
                TTSManager.Instance.SetPitch(1f);
            }           
        }        
    }

    // Update is called once per frame
    void Update()
    {
        totalTimer = WorkoutTimerManager.timer;
        UpdateTimerDisplay();

        if (WorkoutSoundManager.isSoundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSprite;
        }

        else
        {
            soundButton.GetComponent<Image>().sprite = muteSprite;
        }

        if (isWorkoutTimeCounterRunning)
        {
            workoutTimeCounter += Time.deltaTime;

            if(workoutTimeCounter >= workoutTime)
            {
                isWorkoutTimeCounterRunning = false;
                workoutTimeCounter = 0;
                GetComponent<ExercisesPanelTransition>().TransitionNext();

                return;
            }

            UpdateWorkoutTimerDisplay();
        }
    }

    public void SetStartWorkoutData(int id, string title, string time, string timeType, int total, GameObject currentPanel, GameObject infoPanel, GameObject backPanel)
    {
        if (timeType == "step")
        {
            _time.text = "X " + time;
            workoutScroller.SetActive(true);
        }
        else
        {
            _time.text = time;
            workoutTimer.gameObject.SetActive(true);
            isTimedWorkout = true;
            workoutTime = ConvertToSeconds(time);
        }

        _title.text = title;
        titleTTS = title.ToLower().Replace("ups", "up's");

        string titleWithoutSpaces = title.Replace(" ", "").Replace("-", "").Replace("&", "").Replace("'", "");
        _image.GetComponent<SpriteAnimator>().folderName = titleWithoutSpaces;

        PanelTransition infoPanelTransition = infoBtn.GetComponent<PanelTransition>();
        infoPanelTransition.currentPanel = currentPanel;
        infoPanelTransition.nextPanel = infoPanel;

        infoBtn.onClick.RemoveAllListeners();
        infoBtn.onClick.AddListener(() => infoPanelTransition.ExerciseTransitionUp(id - 1, PreviousPanelType.StartWorkout));
        infoBtn.onClick.AddListener(WorkoutTimerManager.StopTimer);
        infoBtn.onClick.AddListener(() => isWorkoutTimeCounterRunning = false);

        soundButton.onClick.RemoveAllListeners();
        soundButton.onClick.AddListener(WorkoutSoundManager.ChangeSoundStatus);

        PanelTransition backPanelTransition = backBtn.GetComponent<PanelTransition>();
        backPanelTransition.currentPanel = currentPanel;
        backPanelTransition.nextPanel = backPanel;

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(backPanelTransition.FadeOut);
        backBtn.onClick.AddListener(WorkoutTimerManager.ResetTimer);
        backBtn.onClick.AddListener(() =>
        {
            isWorkoutTimeCounterRunning = false;
            workoutTimeCounter = 0;
        });

        ExercisesPanelTransition exercisePanelTransition = GetComponent<ExercisesPanelTransition>();
        exercisePanelTransition.parentObject = currentPanel;

        previousBtn.onClick.RemoveAllListeners();
        previousBtn.onClick.AddListener(exercisePanelTransition.TransitionPrevious);

        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(exercisePanelTransition.TransitionNext);

        workoutCompleteBtn.onClick.RemoveAllListeners();
        workoutCompleteBtn.onClick.AddListener(exercisePanelTransition.TransitionNext);

        for(int i = 0; i < total; i++)
        {
            GameObject progressTrackerBar = Instantiate(progressTrackerBarPrefab);
            progressTrackerBar.transform.SetParent(progressTrackerParent.transform);

            if(i < id - 1)
            {
                progressTrackerBar.GetComponent<Image>().sprite = progressTrackerBarCompleteSprite;
            }
        }

        dataSet = true;
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(totalTimer / 60f);
        int seconds = Mathf.FloorToInt(totalTimer % 60f);

        // Format the time as MM:SS
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateWorkoutTimerDisplay()
    {
        timedExerciseTimerText.text = Mathf.FloorToInt(workoutTimeCounter).ToString() + "/" + workoutTime.ToString();
        workoutTimerFill.fillAmount = Mathf.FloorToInt(workoutTimeCounter) / (float)workoutTime;
    }

    int ConvertToSeconds(string timeString)
    {
        string[] parts = timeString.Split(':');

        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);

        return minutes * 60 + seconds;
    }
}
