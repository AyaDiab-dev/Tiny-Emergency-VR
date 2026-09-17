using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ClinicUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject locomotion;
    public TrackedPoseDriver headTracking;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        pausePanel.SetActive(false);
        pauseButton.SetActive(false);

        if (locomotion != null)
            locomotion.SetActive(false);

        if (headTracking != null)
            headTracking.trackingType =
                TrackedPoseDriver.TrackingType.RotationOnly;
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);

        if (locomotion != null)
            locomotion.SetActive(true);

        if (headTracking != null)
            headTracking.trackingType =
                TrackedPoseDriver.TrackingType.RotationAndPosition;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);

        if (locomotion != null)
            locomotion.SetActive(false);
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);

        if (locomotion != null)
            locomotion.SetActive(true);
    }
}