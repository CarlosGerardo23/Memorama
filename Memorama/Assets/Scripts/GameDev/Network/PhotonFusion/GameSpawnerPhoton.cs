using Fusion;
using Fusion.Sockets;
using GameDev.Network.Photon.Mobile.Inputs;
using GameDev.Behaviour2D.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;

namespace GameDev.Network.Photon
{
    public class GameSpawnerPhoton : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkPrefabRef _playerPrefab;
        [SerializeField] private InputTouchControls _touchControlls;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private NetworkRunner _runner;

        private Vector3 _endPosition = default;
        private Vector3 _startPosition = default;
        private bool _inputEnable= false;
        async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        private void OnEnable()
        {
            _touchControlls.Enable();
            _touchControlls.OnStartedTouchPosition += OnFingerUP;
        }
        private void OnDisable()
        {
            _touchControlls.Disable();
        }
        private void OnGUI()
        {
            if (_runner == null)
            {
                if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
                {
                    StartGame(GameMode.Host);
                }
                if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
                {
                    StartGame(GameMode.Client);
                }
            }
        }
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                // Create a unique position for the player
                Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                // Keep track of the player avatars so we can remove it when they disconnect
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            // Find and remove the players avatar
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            //TODO:Make more modular the input for getting any struct INetworkInput
            if (_inputEnable)
            {
                _inputEnable = false;
                var data = new MobileNetworkInputData();
                data.fingerStartPosition = _startPosition;
                data.fingerEndPosition = _endPosition;
                input.Set(data);
            }

        }
        public void OnConnectedToServer(NetworkRunner runner)
        {

        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {

        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {

        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {

        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {

        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {

        }



        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {

        }



        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {

        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {

        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {

        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {

        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {

        }

        private void OnFingerUP(Vector2 position)
        {
            print("Interact");
            _startPosition = position;
            _inputEnable = true;
        }
    }
}