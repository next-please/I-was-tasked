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
        #endregion

        #region Public Methods
        public void updateState(string state)
        {
            Debug.LogFormat("NetworkManager: {0} has called updateState()", PhotonNetwork.LocalPlayer.NickName);

            // TODO: Remove
            state = PhotonNetwork.LocalPlayer.NickName;

            RaiseEvent(UPDATE_STATE, state);
        }

        public void updateStateWithPermission(string state)
        {
            Debug.LogFormat("NetworkManager: {0} has called updateStateWithPermission()", PhotonNetwork.LocalPlayer.NickName);

            // TODO: Remove
            state = PhotonNetwork.LocalPlayer.NickName;

            if (PhotonNetwork.IsMasterClient)
            {
                bool updated = UpdateStateIfPermissionGranted(state);
                if (!updated) { Debug.LogFormat("NetworkManager: {0}'s request has been rejected. Current state is {1}.", PhotonNetwork.LocalPlayer.NickName, this.state); }
                return;
            }

            RaiseEvent(UPDATE_STATE_WITH_PERMISSION, state);
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

        bool UpdateStateIfPermissionGranted(string newState)
        {
            // TODO: Replace with actual permission check
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
                default: break;
            }
        }
        #endregion
    }
}
