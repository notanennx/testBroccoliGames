using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GUI : MonoBehaviour
{
    // Awake
    public static GUI i;
    void Awake()
    {
        i = this;
    }

    // Update
    public void UpdateTextPinpointed(TMP_Text targetText)
    {
        targetText.text = CameraController.i.PinPoint(CameraController.i.pinpointPos);
    }
}
