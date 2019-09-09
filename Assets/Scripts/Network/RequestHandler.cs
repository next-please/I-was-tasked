using UnityEngine;

namespace Com.Nextplease.IWT
{
    public class RequestHandler : MonoBehaviour
    {
        #region Action Types (move out when too large)
        private const string CLASS_NAME = "RequestHandler";
        private const int MOVE_FROM_BENCH_TO_BOARD = 0;
        private const int INIT_PHASE = 10;
        private const int MARKET_PHASE = 11;
        private const int PRECOMBAT_PHASE = 12;
        #endregion

        #region Manager References

        public RoomManager roomManager;
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
            switch (req.GetActionType())
            {
                case MOVE_FROM_BENCH_TO_BOARD:
                    PieceMovementData data = (PieceMovementData)req.GetData();
                    if (arrangementManager.IsValidBenchToBoard(data.player, data.piece, data.tile))
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: MOVE_BENCH_TO_BOARD - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;

                case INIT_PHASE:
                    if (roomManager.IsRoomFull())
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: INIT_PHASE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;

                case MARKET_PHASE:
                    if(networkManager.IsMasterClient())
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: MARKET_PHASE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;

                case PRECOMBAT_PHASE:
                    if(networkManager.IsMasterClient())
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: PRECOMBAT_PHASE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;

                default:
                    Debug.LogErrorFormat("{0}: {1} issued request of invalid action type {2}", CLASS_NAME, req.GetRequester(), req.GetActionType());
                    return null;
            }
        }

        /// <summary>
        /// ExecuteRequest utilises various Managers to execute a request given if it is approved. For all clients.
        /// </summary>
        /// <param name="req"></param>
        public void ExecuteRequest(Request req)
        {
            if (!req.IsApproved())
            {
                Debug.LogFormat("{0}: Request from {1} is rejected", CLASS_NAME, req.GetRequester());
                return;
            }

            switch (req.GetActionType())
            {
                case MOVE_FROM_BENCH_TO_BOARD:
                    PieceMovementData data_0 = (PieceMovementData)req.GetData();
                    arrangementManager.MoveBenchToBoard(data_0.player, data_0.piece, data_0.tile);
                    Debug.LogFormat("{0}: Executing MOVE_BENCH_TO_BOARD from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case INIT_PHASE:
                    phaseManager.TryIntialize();
                    Debug.LogFormat("{0}: Executing INIT_PHASE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case MARKET_PHASE:
                    phaseManager.MarketPhase();
                    Debug.LogFormat("{0}: Executing MARKET_PHASE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case PRECOMBAT_PHASE:
                    Debug.LogFormat("{0}: Executing PRECOMBAT_PHASE from {1}", CLASS_NAME, req.GetRequester());
                    break;

                default:
                    Debug.LogErrorFormat("{0}: {1} issued request of invalid action type {2}", req.GetRequester(), req.GetActionType());
                    break;
            }

        }
        #endregion
    }
}
