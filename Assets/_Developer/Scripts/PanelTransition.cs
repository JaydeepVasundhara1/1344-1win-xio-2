using UnityEngine;
using DG.Tweening;  // Import DOTween
using System.Collections.Generic;

public enum PreviousPanelType
{
    ExerciseList,
    StartWorkout
}

public class PanelTransition : MonoBehaviour
{
    //public GameObject[] panels;  // Array to hold all panels
    public GameObject currentPanel;
    public GameObject nextPanel;
    private float moveDuration = 0.25f; // Duration of the transition
    private Vector3 offscreenPositionLeft = new Vector3(-1080, 0, 0); // Left off-screen position
    private Vector3 offscreenPositionRight = new Vector3(1080, 0, 0); // Right off-screen position
    private Vector3 offscreenPositionDown = new Vector3(0, -1920, 0); // Down off-screen position
    private Vector3 offscreenPositionUp = new Vector3(0, 1920, 0);
    private Vector3 onscreenPosition = Vector3.zero; // Final position (onscreen)

    // Transition Panel from left to right (sideways)
    public void TransitionSideways()
    {
        nextPanel.transform.SetAsLastSibling();

        // Set the initial position for the next panel (off-screen)
        nextPanel.transform.localPosition = offscreenPositionLeft;

        // Activate the next panel (but keep the current one active until the transition completes)
        nextPanel.SetActive(true);

        // Animate the next panel in (move it on-screen)
        nextPanel.transform.DOLocalMove(onscreenPosition, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            // Once the next panel has finished transitioning, deactivate the current panel
            currentPanel.SetActive(false);
        });
    }

    // Transition Panel from down to up (vertical)
    public void TransitionVerticalUp()
    {
        nextPanel.transform.SetAsLastSibling();

        // Set the initial position for the next panel (off-screen)
        nextPanel.transform.localPosition = offscreenPositionDown;

        // Activate the next panel (but keep the current one active until the transition completes)
        nextPanel.SetActive(true);

        //Animate the next panel in (move it on-screen)
        nextPanel.transform.DOLocalMove(onscreenPosition, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            // Once the next panel has finished transitioning, deactivate the current panel
            currentPanel.SetActive(false);
        });
    }

    public void TransitionVerticalDown()
    {
        // Activate the next panel (but keep the current one active until the transition completes)
        nextPanel.SetActive(true);

        // Animate the next panel in (move it on-screen)
        currentPanel.transform.DOLocalMove(offscreenPositionDown, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            // Once the next panel has finished transitioning, deactivate the current panel
            currentPanel.SetActive(false);
        });
    }

    public void FadeIn()
    {
        nextPanel.transform.SetAsLastSibling();

        nextPanel.transform.localPosition = onscreenPosition;

        nextPanel.SetActive(true);

        CanvasGroup currentCanvasGroup = nextPanel.GetComponent<CanvasGroup>();
        if (currentCanvasGroup == null)
        {
            currentCanvasGroup = nextPanel.AddComponent<CanvasGroup>(); // Add CanvasGroup if it's missing
        }

        // Ensure the current panel starts fully visible (alpha = 1)
        currentCanvasGroup.alpha = 0f;

        // Fade out the current panel during the transition
        currentCanvasGroup.DOFade(1, 0.1f).OnComplete(() =>
        {
            // Once the next panel has finished transitioning, deactivate the current panel
            currentPanel.SetActive(false);
        });
    }

    public void FadeOut()
    {
        nextPanel.SetActive(true);

        CanvasGroup currentCanvasGroup = currentPanel.GetComponent<CanvasGroup>();
        if (currentCanvasGroup == null)
        {
            currentCanvasGroup = currentPanel.AddComponent<CanvasGroup>(); // Add CanvasGroup if it's missing
        }

        // Ensure the current panel starts fully visible (alpha = 1)
        currentCanvasGroup.alpha = 1f;

        // Fade out the current panel during the transition
        currentCanvasGroup.DOFade(0, 0.1f).OnComplete(() =>
        {
            // Once the next panel has finished transitioning, deactivate the current panel
            currentPanel.SetActive(false);
        });
    }

    public void ExerciseTransitionUp(int index, PreviousPanelType previousPanelType)
    {
        List<GameObject> exercisePanels = new List<GameObject>();
        GameObject panelToActivate = null;

        foreach(Transform t in nextPanel.transform)
        {
            exercisePanels.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }

        for(int i = 0; i < exercisePanels.Count; i++)
        {
            if(i == index)
            {
                panelToActivate = exercisePanels[i];

                panelToActivate.GetComponent<ExerciseDescriptionData>().SetDescriptionBackBtnPreviousPanel(previousPanelType);
            }
        }

        nextPanel.transform.SetAsLastSibling();

        // Set the initial position for the next panel (off-screen)
        panelToActivate.SetActive(true);
        panelToActivate.transform.localPosition = new Vector3(0, 0, 0);
        nextPanel.transform.localPosition = offscreenPositionDown;

        // Activate the next panel (but keep the current one active until the transition completes)
        nextPanel.SetActive(true);

        // Animate the next panel in (move it on-screen)
        nextPanel.transform.DOLocalMove(onscreenPosition, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            // Once the next panel has finished transitioning, deactivate the current panel
            currentPanel.SetActive(false);
        });
    }

    public void FadeInToFirstChild()
    {
        nextPanel.transform.SetAsLastSibling();

        nextPanel.transform.localPosition = onscreenPosition;

        nextPanel.SetActive(true);

        foreach(Transform t in nextPanel.transform)
        {
            t.gameObject.SetActive(false);
        }

        nextPanel.transform.GetChild(0).localPosition = Vector3.zero;
        nextPanel.transform.GetChild(0).gameObject.SetActive(true);

        CanvasGroup currentCanvasGroup = nextPanel.GetComponent<CanvasGroup>();
        if (currentCanvasGroup == null)
        {
            currentCanvasGroup = nextPanel.AddComponent<CanvasGroup>(); // Add CanvasGroup if it's missing
        }

        // Ensure the current panel starts fully visible (alpha = 1)
        currentCanvasGroup.alpha = 0f;

        // Fade out the current panel during the transition
        currentCanvasGroup.DOFade(1, 0.1f).OnComplete(() =>
        {
            // Once the next panel has finished transitioning, deactivate the current panel
            currentPanel.SetActive(false);
        });
    }
}
