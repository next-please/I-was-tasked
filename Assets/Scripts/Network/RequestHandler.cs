using UnityEngine;

namespace Com.Nextplease.IWT
{
    public class RequestHandler : MonoBehaviour {
        #region Action Types (move out when too large)
        private const int MOVE_FROM_BENCH_TO_BOARD = 0;
        #endregion

        #region Manager References

        private readonly NetworkManager networkManager;
        public ArrangementManager arrangementManager;

        #endregion

        RequestHandler()
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
                case MOVE_FROM_BENCH_TO_BOARD:
                    PieceMovementData data = (PieceMovementData)req.GetData();
                    arrangementManager.TryMoveBenchToBoard(data.player, data.piece, data.tile);
                    break;
                default:
                    Debug.LogErrorFormat("RequestHandler: {0} issued request of invalid action type {1}", req.GetRequester(), req.GetActionType());
                    break;
            }

        }
        #endregion

    }
}
