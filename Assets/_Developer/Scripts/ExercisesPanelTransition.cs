using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ExercisesPanelTransition : MonoBehaviour
{
    public GameObject parentObject;
    private List<GameObject> panels = new List<GameObject>();
    public GameObject currentPanel;
    private GameObject nextPanel;
    private GameObject previousPanel;
    private float moveDuration = 0.2f; // Duration of the transition
    private Vector3 offscreenPositionLeft = new Vector3(-1080, 0, 0); // Left off-screen position
    private Vector3 offscreenPositionRight = new Vector3(1080, 0, 0); // Right off-screen position
    //private Vector3 offscreenPositionDown = new Vector3(0, -1920, 0); // Down off-screen position
    private Vector3 onscreenPosition = Vector3.zero; // Final position (onscreen)

    private void Start()
    {
        foreach(Transform panel in parentObject.transform)
        {
            panels.Add(panel.gameObject);
        }

        for (int i = 0; i < panels.Count; i++)
        {
            if(panels[i] == currentPanel)
            {
                if(i < panels.Count - 1)
                {
                    nextPanel = panels[i + 1];
                }

                if(i > 0)
                {
                    previousPanel = panels[i - 1];
                }
            }
        }
    }

    // Transition Panel from left to right (sideways)
    public void TransitionNext()
    {
        if(nextPanel != null)
        {
            nextPanel.transform.localPosition = offscreenPositionRight;

            // Activate the next panel (but keep the current one active until the transition completes)
            nextPanel.SetActive(true);

            currentPanel.transform.DOLocalMove(offscreenPositionLeft, moveDuration);

            // Animate the next panel in (move it on-screen)
            nextPanel.transform.DOLocalMove(onscreenPosition, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                // Once the next panel has finished transitioning, deactivate the current panel
                currentPanel.SetActive(false);
            });
        }        
    }

    public void TransitionPrevious()
    {
        if (previousPanel != null)
        {
            previousPanel.transform.localPosition = offscreenPositionLeft;

            // Activate the next panel (but keep the current one active until the transition completes)
            previousPanel.SetActive(true);

            previousPanel.transform.DOLocalMove(onscreenPosition, moveDuration);

            // Animate the next panel in (move it on-screen)
            currentPanel.transform.DOLocalMove(offscreenPositionRight, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                // Once the next panel has finished transitioning, deactivate the current panel
                currentPanel.SetActive(false);
            });
        }
    }
}
