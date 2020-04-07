using UnityEngine;
using UnityEngine.UI;

namespace HapticFeedback {
    public sealed class ManagerApplication : MonoBehaviour {
        public Text infoTextBox;
        private string _platformString;

        /// <summary>
        /// On Awake, we initialize our iOS haptic.
        /// Of course, this only needs to be done when on iOS, or targeting iOS. 
        /// A test will be done and this method will do nothing if running on anything else
        /// </summary>
        private void Awake() {
            Manager.iOSInitializeHaptics();
        }

        /// <summary>
        /// On Start, we display our debug information
        /// </summary>
        private void Start() {
            DisplayInformation();
        }

        /// <summary>
        /// Displays the debug information (API version on Android, iOS sdk version, and error message otherwise)
        /// </summary>
        private void DisplayInformation() {
            if (Manager.IsAndroid) {
                _platformString = "API version " + Manager.AndroidSDKVersion();
            }
            else if (Manager.IsiOS) {
                _platformString = "iOS " + Manager.iOSSDKVersion();
            }
            else {
                _platformString = Application.platform + ", not supported for now.";
            }

            infoTextBox.text = "Platform : " + _platformString;
        }

        /// <summary>
        /// On Disable, we release our iOS haptic (to save memory and avoid garbage).
        /// Of course, this only needs to be done when on iOS, or targeting iOS. 
        /// A test will be done and this method will do nothing if running on anything else
        /// </summary>
        private void OnDisable() {
            Manager.iOSReleaseHaptics();
        }

        /// <summary>
        /// The following methods are bound (via the inspector) to buttons in the demo scene, and will call the corresponding vibration methods
        /// </summary>
        /// <summary>
        /// Triggers the default Unity vibration, without any control over duration, pattern or amplitude
        /// </summary>
        public void TriggerDefault() {
#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        /// <summary>
        /// Triggers the default Vibrate method, which will result in a medium vibration on Android and a medium impact on iOS
        /// </summary>
        public void TriggerVibrate() {
            Manager.Vibrate();
        }

        /// <summary>
        /// Triggers the selection haptic feedback, a light vibration on Android, and a light impact on iOS
        /// </summary>
        public void TriggerSelection() {
            Manager.Haptic(HapticTypes.Selection);
        }

        /// <summary>
        /// Triggers the success haptic feedback, a light then heavy vibration on Android, and a success impact on iOS
        /// </summary>
        public void TriggerSuccess() {
            Manager.Haptic(HapticTypes.Success);
        }

        /// <summary>
        /// Triggers the warning haptic feedback, a heavy then medium vibration on Android, and a warning impact on iOS
        /// </summary>
        public void TriggerWarning() {
            Manager.Haptic(HapticTypes.Warning);
        }

        /// <summary>
        /// Triggers the failure haptic feedback, a medium / heavy / heavy / light vibration pattern on Android, and a failure impact on iOS
        /// </summary>
        public void TriggerFailure() {
            Manager.Haptic(HapticTypes.Failure);
        }

        /// <summary>
        /// Triggers a light impact on iOS and a short and light vibration on Android.
        /// </summary>
        public void TriggerLightImpact() {
            Manager.Haptic(HapticTypes.LightImpact);
        }

        /// <summary>
        /// Triggers a medium impact on iOS and a medium and regular vibration on Android.
        /// </summary>
        public void TriggerMediumImpact() {
            Manager.Haptic(HapticTypes.MediumImpact);
        }

        /// <summary>
        /// Triggers a heavy impact on iOS and a long and heavy vibration on Android.
        /// </summary>
        public void TriggerHeavyImpact() {
            Manager.Haptic(HapticTypes.HeavyImpact);
        }
    }
}