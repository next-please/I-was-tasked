using UnityEngine;

namespace Com.Nextplease.IWT
{
    public class RequestHandler : MonoBehaviour {
        #region Action Types (move out when too large)
        private const int MOVE_FROM_BENCH_TO_BOARD = 0;
        private const int INIT_PHASE = 10;
        #endregion

        #region Manager References

        public PhaseManager phaseManager;
        public NetworkManager networkManager;
        public ArrangementManager arrangementManager;

        #endregion

        #region Public Methods
        /// <summary>
        /// SendRequest sends an incoming request to the NetworkManager for synchronisation between clients.
        /// </summary>
        /// <param name="req"></param>
        public void SendRequest(Request req)
        {
            req.SetRequester(this.networkManager.GetLocalPlayerID());
            this.networkManager.ProcessRequest(req);
        }

        /// <summary>
        /// ValidateRequest validates requests using the various Managers. Only for MasterClient.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Request ValidateRequest(Request req)
        {
            switch(req.GetActionType())
                {
                    case MOVE_FROM_BENCH_TO_BOARD:
                        PieceMovementData data = (PieceMovementData)req.GetData();
                        if(arrangementManager.IsValidBenchToBoard(data.player, data.piece, data.tile))
                        {
                            req.Approve();
                        }
                        Debug.Log("here me dude");
                        return req;

                case INIT_PHASE:
                    req.Approve();
                    return req;

                    default:
                        Debug.LogErrorFormat("RequestHandler: {0} issued request of invalid action type {1}", req.GetRequester(), req.GetActionType());
                        return null;
                }
        }

        /// <summary>
        /// ExecuteRequest utilises various Managers to execute a request given if it is approved. For all clients.
        /// </summary>
        /// <param name="req"></param>
        public void ExecuteRequest(Request req)
        {
            Debug.Log(req.IsApproved());
            if(!req.IsApproved()) { return; }
            switch(req.GetActionType())
            {
                case MOVE_FROM_BENCH_TO_BOARD:
                    PieceMovementData data_0 = (PieceMovementData)req.GetData();
                    arrangementManager.MoveBenchToBoard(data_0.player, data_0.piece, data_0.tile);
                    break;
                case INIT_PHASE:
                    PhaseManagementData data_10 = (PhaseManagementData)req.GetData();
                    phaseManager.SetNumPlayers(data_10.numPlayers);
                    phaseManager.TryIntialize();
                    break;

                default:
                    Debug.LogErrorFormat("RequestHandler: {0} issued request of invalid action type {1}", req.GetRequester(), req.GetActionType());
                    break;
            }

        }
        #endregion
    }
}
