using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AutoDiscovery;
using UnityEngine;
using Mobge.Test;
using UnityEngine.UI;

namespace HapticFeedback {
    public class VibrationServer : MonoBehaviour {
        [SerializeField] private ushort port;
        [SerializeField] private Text IPText;

        private Manager.HapticPattern _pattern;
        private Server _autoDiscoveryServer;

        public void Awake() {
            StartVibrationServer(defaultToRegularVibrate: true);
            IPText.text = DiscoverLocalIP();
            _autoDiscoveryServer = new Server("VibeServer", port) {
                // ServerData = "Server on " + Dns.GetHostName()
            };
            _autoDiscoveryServer.Start();
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

        private void StartVibrationServer(bool defaultToRegularVibrate = false) {
            var s = HttpCommandListener.Shared;
            s.RegisterMethod("vibrate", (request, response) => {
                _pattern = Parse.VibrationQuery(request);
                Manager.CustomHaptic(_pattern, defaultToRegularVibrate);
                s.SendResponse("Sent vibration " + DateTime.Now, response);
            });
            s.Start(port);
        }

        private void OnDestroy() {
            _autoDiscoveryServer.Stop();
        }
    }
}