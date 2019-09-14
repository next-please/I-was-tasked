using UnityEngine;

namespace Com.Nextplease.IWT
{
    public class RequestHandler : MonoBehaviour
    {
        #region Action Types (move out when too large)
        private const string CLASS_NAME = "RequestHandler";
        private const int MOVE_FROM_BENCH_TO_BOARD = 0;
        private const int BUY_PIECE = 5;
        private const int SELL_PIECE = 6;
        private const int INIT_PHASE = 10;
        private const int MARKET_PHASE = 11;
        private const int PRECOMBAT_PHASE = 12;
        private const int POSTCOMBAT_PHASE = 13;


        private const int UPGRADE_INCOME = 100;
        private const int UPGRADE_MARKET_RARITY = 101;
        private const int UPGRADE_MARKET_SIZE = 102;
        private const int UPGRADE_ARMY_SIZE = 103;
        #endregion

        #region Manager References

        public RoomManager roomManager;
        public PhaseManager phaseManager;
        public NetworkManager networkManager;
        public ArrangementManager arrangementManager;
        public InventoryManager inventoryManager;
        public TransactionManager transactionManager;
        public MarketManager marketManager;

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
                    PieceMovementData data_0 = (PieceMovementData)req.GetData();
                    if (arrangementManager.IsValidBenchToBoard(data_0.player, data_0.piece, data_0.tile))
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
                    if (IsMasterClient(req))
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: MARKET_PHASE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;

                case PRECOMBAT_PHASE:
                    if (IsMasterClient(req))
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: PRECOMBAT_PHASE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                case POSTCOMBAT_PHASE:
                    if (IsMasterClient(req))
                       // TODO: wait for all clients to finish before approving
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: POSTCOMBAT_PHASE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                case BUY_PIECE:
                    if(IsMasterClient(req))
                    {
                        PieceTransactionData data_5 = (PieceTransactionData)req.GetData();
                        if(transactionManager.IsValidPurchase(data_5.player, data_5.price))
                        {
                            req.Approve();
                        }
                    }
                    Debug.LogFormat("{0}: BUY_PIECE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                case SELL_PIECE:
                    if(IsMasterClient(req) && transactionManager.IsValidSale()) { req.Approve(); }
                    Debug.LogFormat("{0}: SELL_PIECE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                case UPGRADE_INCOME:
                    UpgradeIncomeData data_100 = req.GetData() as UpgradeIncomeData;
                    if (transactionManager.CanPurchaseIncreasePassiveIncome(data_100.player))
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: UPGRADE_INCOME - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                case UPGRADE_MARKET_RARITY:
                    UpgradeMarketRarityData data_101 = req.GetData() as UpgradeMarketRarityData;
                    if (transactionManager.CanPurchaseIncreaseMarketRarity(data_101.player))
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: UPGRADE_MARKET_RARITY - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                case UPGRADE_MARKET_SIZE:
                    UpgradeMarketSizeData data_102 = req.GetData() as UpgradeMarketSizeData;
                    if (transactionManager.CanPurchaseIncreaseMarketSize(data_102.player))
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: UPGRADE_MARKET_SIZE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                case UPGRADE_ARMY_SIZE:
                    UpgradeArmySizeData data_103 = req.GetData() as UpgradeArmySizeData;
                    if (transactionManager.CanPurchaseIncreaseArmySize(data_103.player))
                    {
                        req.Approve();
                    }
                    Debug.LogFormat("{0}: UPGRADE_ARMY_SIZE - approved: {1}", CLASS_NAME, req.IsApproved());
                    return req;
                default:
                    Debug.LogErrorFormat("{0}: {1} issued request of invalid action type {2}", CLASS_NAME, req.GetRequester(), req.GetActionType());
                    return req;
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
                    PhaseManagementData data_10 = req.GetData() as PhaseManagementData;
                    phaseManager.Initialize(data_10.numPlayers);
                    Debug.LogFormat("{0}: Executing INIT_PHASE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case MARKET_PHASE:
                    MarketManagementData data_11 = (MarketManagementData)req.GetData();
                    marketManager.SetMarketItems(data_11.pieces);
                    phaseManager.StartMarketPhase();
                    Debug.LogFormat("{0}: Executing MARKET_PHASE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case PRECOMBAT_PHASE:
                    phaseManager.StartPreCombat();
                    Debug.LogFormat("{0}: Executing PRECOMBAT_PHASE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case POSTCOMBAT_PHASE:
                    phaseManager.StartPostCombat();
                    Debug.LogFormat("{0}: Executing POSTCOMBAT_PHASE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case BUY_PIECE:
                    PieceTransactionData data_5 = (PieceTransactionData)req.GetData();
                    transactionManager.PurchaseMarketPieceToBench(data_5.player, data_5.piece, data_5.price);
                    Debug.LogFormat("{0}: Executing BUY_PIECE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case SELL_PIECE:
                    PieceTransactionData data_6 = (PieceTransactionData)req.GetData();
                    transactionManager.SellBenchPiece(data_6.player, data_6.piece);
                    Debug.LogFormat("{0}: Executing SELL_PIECE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case UPGRADE_INCOME:
                    UpgradeIncomeData data_100 = req.GetData() as UpgradeIncomeData;
                    transactionManager.PurchaseIncreasePassiveIncome(data_100.player);
                    Debug.LogFormat("{0}: Executing UPGRADE_INCOME from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case UPGRADE_MARKET_RARITY:
                    UpgradeMarketRarityData data_101 = req.GetData() as UpgradeMarketRarityData;
                    transactionManager.PurchaseIncreaseMarketRarity(data_101.player);
                    Debug.LogFormat("{0}: Executing UPGRADE_MARKET_RARITY from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case UPGRADE_MARKET_SIZE:
                    UpgradeMarketSizeData data_102 = req.GetData() as UpgradeMarketSizeData;
                    transactionManager.PurchaseIncreaseMarketSize(data_102.player);
                    Debug.LogFormat("{0}: Executing UPGRADE_MARKET_SIZE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                case UPGRADE_ARMY_SIZE:
                    UpgradeArmySizeData data_103 = req.GetData() as UpgradeArmySizeData;
                    transactionManager.PurchaseIncreaseArmySize(data_103.player);
                    Debug.LogFormat("{0}: Executing UPGRADE_ARMY_SIZE from {1}", CLASS_NAME, req.GetRequester());
                    break;
                default:
                    Debug.LogErrorFormat("{0}: {1} issued request of invalid action type {2}", req.GetRequester(), req.GetActionType());
                    break;
            }

        }
        #endregion

        #region Private Methods
        private bool IsMasterClient(Request req)
        {
            // note: not required but for sanity
            return networkManager.IsMasterClient() && req.GetRequester() == networkManager.GetLocalPlayerID();
        }
        #endregion
    }
}
