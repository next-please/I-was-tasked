using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;
using Random = System.Random;

namespace Com.Nextplease.IWT
{
    public class NetworkManager : MonoBehaviour, IOnEventCallback
    {
        #region Action Codes
        private const byte UPDATE_STATE = 0;
        private const byte UPDATE_STATE_WITH_PERMISSION = 1;
        private const byte UPDATE_READY = 2;
        #endregion

        #region Private Fields
        // TODO: Remove
        private string state = "";

        // Setting for TCP Preference 
        private readonly SendOptions SEND_OPTIONS = new SendOptions { Reliability = true };

        // Ready State Variables - Only Used by Master Client
        private int readyCount = 0;
        private HashSet<string> readyIds = new HashSet<string>();
        #endregion

        #region Monobehaviour Methods
        void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        void Start()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Trigger a state update without permission from master client.
        /// </summary>
        /// <param name="state"></param>
        public void updateState(string state)
        {
            Debug.LogFormat("NetworkManager: {0} has called updateState()", PhotonNetwork.LocalPlayer.NickName);

            // TODO: Remove
            state = PhotonNetwork.LocalPlayer.NickName;

            RaiseEvent(UPDATE_STATE, state);
        }

        /// <summary>
        /// Trigger a state update only after permission from master client is granted.
        /// </summary>
        /// <param name="state"></param>
        public void updateStateWithPermission(string state)
        {
            Debug.LogFormat("NetworkManager: {0} has called updateStateWithPermission()", PhotonNetwork.LocalPlayer.NickName);

            // TODO: Remove
            state = PhotonNetwork.LocalPlayer.NickName;

            if (PhotonNetwork.IsMasterClient)
            {
                // TODO: Change UpdateStateIfPermissionGranted to actual permission check
                bool updated = UpdateStateIfPermissionGranted(state);
                if (!updated) { Debug.LogFormat("NetworkManager: {0}'s request has been rejected. Current state is {1}.", PhotonNetwork.LocalPlayer.NickName, this.state); }
                return;
            }

            RaiseEvent(UPDATE_STATE_WITH_PERMISSION, state);
        }

        /// <summary>
        /// Trigger an ready state update. 
        /// </summary>
        public void updateReady()
        {
            string myPlayerId = PhotonNetwork.LocalPlayer.NickName;
            Debug.LogFormat("NetworkManager: {0} has indicated ready", myPlayerId);

            if (PhotonNetwork.IsMasterClient)
            {
                CheckReady(myPlayerId);

                if (IsAllReady())
                {
                    // TODO: Proceed with next phase
                    ResetReadyState();
                }
                return;
            }
            RaiseEvent(UPDATE_READY, myPlayerId);
        }
        #endregion

        #region Private Methods
        void RaiseEvent(byte code, string data)
        {
            object[] eventContent = { PhotonNetwork.LocalPlayer.NickName, data };
            RaiseEventOptions raiseEventOptions = null;
            switch (code)
            {
                case UPDATE_STATE_WITH_PERMISSION:
                    raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                    break;
                default:
                    raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                    break;
            }

            // TODO: Remove for Optimisation
            Debug.Assert(raiseEventOptions != null);

            PhotonNetwork.RaiseEvent(code, eventContent, raiseEventOptions, SEND_OPTIONS);
        }

        // TODO: Remove method after replacement
        bool UpdateStateIfPermissionGranted(string newState)
        {
            Random rnd = new Random();
            bool permission = rnd.Next(100) < 50;

            Debug.LogFormat("NetworkManager: Master Client {0} permission result: {1}", PhotonNetwork.LocalPlayer.NickName, permission);

            if (permission)
            {
                RaiseEvent(UPDATE_STATE, newState);
                return true;
            }

            return false;
        }

        void ResetReadyState()
        {
            Debug.Log("NetworkManager: Resetting ready state..");
            this.readyCount = 0;
            this.readyIds.Clear();
        }

        void CheckReady(string playerId)
        {
            if (!readyIds.Contains(playerId))
            {
                readyCount++;
                readyIds.Add(playerId);
            }
        }

        bool IsAllReady()
        {
            bool isAllReady = readyCount >= 3;
            Debug.LogFormat("NetworkManager: Master Client {0} - all ready status: {1}, number of ready players: {2}", PhotonNetwork.LocalPlayer.NickName, isAllReady, readyCount);

            if (isAllReady)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Photon Callbacks
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.CustomData == null) { return; }

            Debug.LogFormat("NetworkManager: {0} has received photon event.", PhotonNetwork.LocalPlayer.NickName);
            object[] data = (object[])photonEvent.CustomData;

            // TODO: Replace with actual state representation
            string requester = (string)data[0];
            string newState = (string)data[1];

            switch (photonEvent.Code)
            {
                case UPDATE_STATE:
                    // TODO: Replace with actual state update code
                    this.state = newState;
                    Debug.LogFormat("NetworkManager: state has been updated to {0}", this.state);

                    break;

                case UPDATE_STATE_WITH_PERMISSION:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.LogFormat("NetworkManager: Master Client {0} is checking permission...", PhotonNetwork.LocalPlayer.NickName);
                        bool updated = UpdateStateIfPermissionGranted(newState);
                        if (!updated) { Debug.LogFormat("NetworkManager: {0}'s request has been rejected. Current state is {1}.", requester, this.state); }
                    }
                    break;
                case UPDATE_READY:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.LogFormat("NetworkManager: Non Master Client {0} is indicating ready...", requester);
                        CheckReady(newState);

                        if (IsAllReady())
                        {
                            // TODO: Proceed with next phase
                            ResetReadyState();
                        }
                    }
                    break;
                default:
                    Debug.LogError("NetworkManager: OnEvent() received photonEvent with invalid event code.");
                    break;
            }
        }
        #endregion
    }
}
