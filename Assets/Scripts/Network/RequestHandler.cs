using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Nextplease.IWT
{
    public class RequestHandler {
        #region Manager References

        private readonly NetworkManager networkManager;

        #endregion

        public RequestHandler()
        {
            this.networkManager = new NetworkManager(this);
        }

        #region Public Methods
        public void SendRequest(Request req)
        {
            this.networkManager.ProcessRequest(req);            
        }

        public void ExecuteRequest(Request req)
        {
            switch(req.GetActionType())
            {
                default:
                    Debug.LogErrorFormat("RequestHandler: {0} issued request of invalid action type {1}", req.GetRequester(), req.GetActionType());
                    break;
            }

        }
        #endregion

    }
}
