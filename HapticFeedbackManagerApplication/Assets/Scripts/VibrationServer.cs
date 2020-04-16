using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Mobge.Test;
using UnityEngine.UI;

namespace HapticFeedback {
    public class VibrationServer : MonoBehaviour {
        [SerializeField] private ushort port;
        [SerializeField] private Text IPText;

        private Manager.HapticPattern _pattern;
        private static float _threshold = 0.1f;


        public void Awake() {
            StartVibrationServer();
            IPText.text = DiscoverIP();
        }

        private string DiscoverIP() {
            var localIp = new StringBuilder("Local IP: ");
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                localIp.Append(endPoint?.Address);
            }

            return localIp.ToString();
        }

        private void StartVibrationServer() {
            var s = HttpCommandListener.Shared;
            s.RegisterMethod("vibrate", (request, response) => {
                ParseVibrationQuery(request, out long[] durations, out int[] amplitudes);
                _pattern = new Manager.HapticPattern(durations, amplitudes, -1);
                Manager.CustomHaptic(_pattern);
                s.SendResponse("Sent vibration " + DateTime.Now, response);
            });

            s.RegisterMethod("update_threshold", (request, response) => {
                _threshold = (float) double.Parse(request.QueryString.Get("t"));
                s.SendResponse("Updated threshold to: " + _threshold + "  " + DateTime.Now, response);
            });


            s.Start(port);
        }

        private static void ParseVibrationQuery(HttpListenerRequest request, out long[] durations, out int[] amplitudes) {
            var time = (float) double.Parse(request.QueryString.Get("t"));
            string queryAmps = request.QueryString.Get("a");
            string[] splicedAmps = queryAmps.Split(',');
            int length = splicedAmps.Length;
            amplitudes = new int[length * 2];
            durations = new long[length * 2];
            // wait vibate wait vibrate wait vibrate...
            for (var i = 1; i < length; i += 2) {
                var normalizedAmp = double.Parse(splicedAmps[i - 1]);
                if (normalizedAmp < _threshold) {
                    // waits
                    amplitudes[i - 1] = (int) (255 * normalizedAmp);
                    durations[i - 1] = (long) (time * 1000);

                    // vibrates
                    amplitudes[i] = 0;
                    durations[i] = 0;
                }
                else {
                    // waits
                    amplitudes[i - 1] = 0;
                    durations[i - 1] = 0;

                    // vibrates
                    amplitudes[i] = (int) (255 * normalizedAmp);
                    durations[i] = (long) (time * 1000);
                }
            }
        }
    }
}