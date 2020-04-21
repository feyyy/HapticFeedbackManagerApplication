using System.Collections;
using System.Collections.Generic;
using HapticFeedback;
using UnityEngine;

namespace HapticFeedback {

    public class VibrationDataObjectTest : MonoBehaviour {
        public VibrationDataObject vibrationDataObject;

        public void OnClick() {
            vibrationDataObject.data.Vibrate();
        }
    }
}