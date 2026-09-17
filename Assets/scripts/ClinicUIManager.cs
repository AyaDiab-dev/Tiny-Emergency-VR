using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ClinicUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject locomotion;
    public TrackedPoseDriver headTracking;

    void Start()
    {
        // قبل START: ممنوع التنقل
        locomotion.SetActive(false);

        // مسموح فقط يلف راسه
        headTracking.trackingType =
            TrackedPoseDriver.TrackingType.RotationOnly;
    }

    public void StartGame()
    {
        // إخفاء القائمة
        mainMenuPanel.SetActive(false);

        // تشغيل الحركة
        locomotion.SetActive(true);

        // إعادة تتبع الرأس الكامل
        headTracking.trackingType =
            TrackedPoseDriver.TrackingType.RotationAndPosition;
    }
}