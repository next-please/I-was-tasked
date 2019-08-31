using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;

namespace Com.Nextplease.IWT {
    public class NetworkManager : MonoBehaviour, IOnEventCallback
    {
        #region Action Codes
        private const byte UPDATE_STATE_CODE = 0;
        private const byte CHECK_PERMISSION_CODE = 1;
        #endregion

        // TODO: Remove
        #region Private Fields
        private string state = "";
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
            // TODO: Remove
            Debug.LogFormat("NetworkManager: {0} has called updateState()", PhotonNetwork.LocalPlayer.NickName);
            state = PhotonNetwork.LocalPlayer.NickName;

            object[] content = { state };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(UPDATE_STATE_CODE, content, raiseEventOptions, sendOptions);
        }

        public void updateStateWithPermission(string state)
        {

        }
        #endregion

        #region Photon Callbacks
        public void OnEvent(EventData photonEvent)
        {
            Debug.LogFormat("NetworkManager: {0} has received photon event.", PhotonNetwork.LocalPlayer.NickName);
            switch (photonEvent.Code)
            {
                case UPDATE_STATE_CODE:
                    object[] data = (object[])photonEvent.CustomData;
                    this.state = (string)data[0];
                    Debug.Log("NetworkManager: state has been updated to " + this.state);
                    break;

                case 1: return;
                default: break;
            }
        }
        #endregion
    }
}
