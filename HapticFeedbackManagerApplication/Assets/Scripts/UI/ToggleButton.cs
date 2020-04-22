using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour {

    [SerializeField] private Transform predefinedVibrationContainer;
    [SerializeField] private Transform customVibrationContainer;
    private bool _isPredefinedActive;

    public void OnClick() {
        _isPredefinedActive = !_isPredefinedActive;
        if (_isPredefinedActive) {
            predefinedVibrationContainer.gameObject.SetActive(true);
            customVibrationContainer.gameObject.SetActive(false);
        } else {
            predefinedVibrationContainer.gameObject.SetActive(false);
            customVibrationContainer.gameObject.SetActive(true);
        }
        
        var scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
    }
}
