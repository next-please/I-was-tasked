using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;
using Random = System.Random;

namespace Com.Nextplease.IWT
{
    public class NetworkManager : MonoBehaviour, IOnEventCallback
    {

        #region Private Fields
        public RequestHandler reqHandler;

        private const byte VALIDATE_ACTION_WITH_MASTER = 0;
        private const byte UPDATE_STATE = 1;

        // Setting for TCP Preference
        private readonly SendOptions SEND_OPTIONS = new SendOptions { Reliability = true };
        #endregion

        #region Public Methods
        public void ProcessRequest(Request req)
        {
            // If MasterClient, validate and update all
            if (PhotonNetwork.IsMasterClient)
            {
                Request processedReq = reqHandler.ValidateRequest(req);
                RaiseEvent(UPDATE_STATE, processedReq);
                return;
            }

            // Else, validate with MasterClient
            RaiseEvent(VALIDATE_ACTION_WITH_MASTER, req);
            return;
        }

        public string GetLocalPlayerID()
        {
            return PhotonNetwork.LocalPlayer.NickName;
        }
        #endregion

        #region Private Methods
        private void RaiseEvent(byte code, Request r)
        {
            byte[] content = Serialization.serializeObject(r);
            RaiseEventOptions raiseEventOptions;
            switch (code)
            {
                case VALIDATE_ACTION_WITH_MASTER:
                    raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                    break;
                default:
                    raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                    break;
            }

            // TODO: Remove
            Debug.Assert(raiseEventOptions != null);
            PhotonNetwork.RaiseEvent(code, content, raiseEventOptions, SEND_OPTIONS);
            Debug.Log("EVENT RAISED");
        }
        #endregion

        #region Photon Setup
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.CustomData == null) { return; }

            Debug.LogFormat("NetworkManager: {0} has received photon event.", PhotonNetwork.LocalPlayer.NickName);

            byte[] content = (byte[])photonEvent.CustomData;

            Request req = (Request)Serialization.deserializeData(content);

            switch (photonEvent.Code)
            {
                case UPDATE_STATE:
                    this.reqHandler.ExecuteRequest(req);
                    break;

                case VALIDATE_ACTION_WITH_MASTER:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        ProcessRequest(req);
                    }
                    else
                    {
                        Debug.LogErrorFormat("NetworkHandler: Validate action sent to non-master client '{0}' from '{1}'.",
                            PhotonNetwork.LocalPlayer.NickName, req.GetRequester());
                    }
                    break;
                default:
                    Debug.LogFormat("NetworkManager: {0} received photonEvent with invalid event code {1}.", PhotonNetwork.LocalPlayer.NickName, photonEvent.Code);
                    break;
            }
        }
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
    }
}
