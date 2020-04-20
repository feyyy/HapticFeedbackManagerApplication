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
            IPText.text = DiscoverLocalIP();
        }

        private string DiscoverLocalIP() {
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
                _pattern = Parse.VibrationQuery(request, _threshold);
                Manager.CustomHaptic(_pattern);
                s.SendResponse("Sent vibration " + DateTime.Now, response);
            });

            s.RegisterMethod("update_threshold", (request, response) => {
                _threshold = (float) double.Parse(request.QueryString.Get("t"));
                s.SendResponse("Updated threshold to: " + _threshold + "  " + DateTime.Now, response);
            });
            s.Start(port);
        }


    }
}