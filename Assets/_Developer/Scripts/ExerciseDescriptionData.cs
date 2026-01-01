using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseDescriptionData : MonoBehaviour
{
    public Button exerciseListBackBtn;
    public Button startWorkoutBackBtn;
    public Image _image;
    public TMP_Text _title;
    public TMP_Text _description;
    public Button previousBtn;
    public Button nextBtn;
    public TMP_Text exerciseNo;
    public GameObject exerciseScroller;

    public PreviousPanelType previousPanelType;

    void Start()
    {
        GetComponent<Image>().color = ColorApplicator.instance.ImageColor_light;
        exerciseListBackBtn.GetComponent<Image>().color = ColorApplicator.instance.TextColor;
        startWorkoutBackBtn.GetComponent<Image>().color = ColorApplicator.instance.TextColor;
        _title.color = ColorApplicator.instance.TextColor;
        exerciseNo.color = ColorApplicator.instance.TextColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetExerciseDescriptionData(int id, string title, string description, int total, GameObject currentPanel, GameObject exerciseListParent, GameObject startWorkoutParent)
    {
        _title.text = title;

        string modifiedDescription = description.Replace("\\r\\n", "\n").Replace("\\n\\n", "\n\n").Replace("\\n", "\n");
        _description.text = modifiedDescription;

        string titleWithoutSpaces = title.Replace(" ", "").Replace("-", "").Replace("&", "").Replace("'", "");
        _image.GetComponent<SpriteAnimator>().folderName = titleWithoutSpaces;

        exerciseNo.text = id + "/" + total;

        PanelTransition exerciseListPanelTransition = exerciseListBackBtn.GetComponent<PanelTransition>();
        exerciseListPanelTransition.currentPanel = currentPanel;
        exerciseListPanelTransition.nextPanel = exerciseListParent;

        exerciseListBackBtn.onClick.RemoveAllListeners();
        exerciseListBackBtn.onClick.AddListener(exerciseListPanelTransition.TransitionVerticalDown);

        PanelTransition startWorkoutPanelTransition = startWorkoutBackBtn.GetComponent<PanelTransition>();
        startWorkoutPanelTransition.currentPanel = currentPanel;
        startWorkoutPanelTransition.nextPanel = startWorkoutParent;

        startWorkoutBackBtn.onClick.RemoveAllListeners();
        startWorkoutBackBtn.onClick.AddListener(startWorkoutPanelTransition.TransitionVerticalDown);
        startWorkoutBackBtn.onClick.AddListener(WorkoutTimerManager.StartTimer);

        ExercisesPanelTransition transitionPrevious = previousBtn.GetComponent<ExercisesPanelTransition>();
        transitionPrevious.parentObject = currentPanel;

        ExercisesPanelTransition transitionNext = nextBtn.GetComponent <ExercisesPanelTransition>();
        transitionNext.parentObject = currentPanel;

        previousBtn.onClick.RemoveAllListeners();
        previousBtn.onClick.AddListener(transitionPrevious.TransitionPrevious);

        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(transitionNext.TransitionNext);


    }

    public void SetDescriptionBackBtnPreviousPanel(PreviousPanelType previousPanelType)
    {
        if (previousPanelType == PreviousPanelType.ExerciseList)
        {
            exerciseScroller.SetActive(true);            
        }


        else if (previousPanelType == PreviousPanelType.StartWorkout)
        {
            exerciseScroller.SetActive(false);
            exerciseListBackBtn.gameObject.SetActive(false);
            startWorkoutBackBtn.gameObject.SetActive(true);
        }
    }
}
