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
        private RequestHandler reqHandler;

        private const byte VALIDATE_ACTION_WITH_MASTER = 0;
        private const byte UPDATE_STATE = 1;

        // Setting for TCP Preference 
        private readonly SendOptions SEND_OPTIONS = new SendOptions { Reliability = true };
        #endregion

        public NetworkManager(RequestHandler reqHandler)
        {
            this.reqHandler = reqHandler;
        }

        #region Public Methods
        public void ProcessRequest(Request req)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (IsValidRequest(req))
                {
                    req.Approve();
                }
                RaiseEvent(UPDATE_STATE, req);
                return;
            }

            RaiseEvent(VALIDATE_ACTION_WITH_MASTER, req);
            return;
        }
        #endregion

        #region Private Methods
        private void RaiseEvent(byte code, Request r)
        {
            object[] content = { r };
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
        }

        // TODO: Implement validity check with for requests given action type @nicholaschuayunzhi 
        bool IsValidRequest(Request r)
        {
            Random rnd = new Random();

            bool isValid = rnd.Next(100) < 50;
            Debug.LogFormat("NetworkManager: Master Client {0} permission result for request from {1}: {2}",
                PhotonNetwork.LocalPlayer.NickName, r.GetRequester(), isValid);
            return isValid;
        }
        #endregion

        #region Photon Setup
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.CustomData == null) { return; }

            Debug.LogFormat("NetworkManager: {0} has received photon event.", PhotonNetwork.LocalPlayer.NickName);

            object[] content = (object[])photonEvent.CustomData;

            Request req = (Request)content[0];

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
