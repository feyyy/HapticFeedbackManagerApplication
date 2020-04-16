using System.Collections;
using System.Collections.Generic;
using HapticFeedback;
using UnityEngine;

public class VibrationDataObjectTest : MonoBehaviour {
    public VibrationDataObject vibrationDataObject;

    public void OnClick() {
        vibrationDataObject.data.Vibrate();
    }
}
