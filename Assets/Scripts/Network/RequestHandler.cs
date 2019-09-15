using UnityEngine;
using static Com.Nextplease.IWT.ActionTypes;

namespace Com.Nextplease.IWT
{
    public class RequestHandler : MonoBehaviour
    {
        #region Private Fields
        private const string CLASS_NAME = "RequestHandler";
        private const bool DEBUG_MODE = true;
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
                    if (arrangementManager.CanMoveBenchToBoard(data_0.player, data_0.piece, data_0.tile))
                    {
                        req.Approve();
                    }
                    break;
                case MOVE_FROM_BOARD_TO_BENCH:
                    BoardToBenchData data_1 = (BoardToBenchData)req.GetData();
                    if (arrangementManager.CanMoveBoardToBench(data_1.player, data_1.piece, data_1.slotIndex))
                    {
                        req.Approve();
                    }
                    break;
                case MOVE_ON_BOARD:
                    PieceMovementData data_2 = (PieceMovementData)req.GetData();
                    if (arrangementManager.CanMovePieceOnBoard(data_2.player, data_2.piece, data_2.tile))
                    {
                        req.Approve();
                    }
                    break;
                case INIT_PHASE:
                    if (roomManager.IsRoomFull())
                    {
                        req.Approve();
                    }
                    break;
                case ROUND_START:
                    if (req.GetRequester() == networkManager.GetLocalPlayerID())
                    {
                        req.Approve();
                    }
                    break;
                case MARKET_PHASE:
                    if (req.GetRequester() == networkManager.GetLocalPlayerID())
                    {
                        req.Approve();
                    }
                    break;
                case PRECOMBAT_PHASE:
                    if (req.GetRequester() == networkManager.GetLocalPlayerID())
                    {
                        req.Approve();
                    }
                    break;
                case POSTCOMBAT_PHASE:
                    if (req.GetRequester() == networkManager.GetLocalPlayerID())
                    // TODO: wait for all clients to finish before approving
                    {
                        req.Approve();
                    }
                    break;
                case BUY_PIECE:
                    PieceTransactionData data_5 = (PieceTransactionData)req.GetData();
                    if (transactionManager.IsValidPurchase(data_5.player, data_5.price))
                    {
                        req.Approve();
                    }
                    break;
                case SELL_PIECE:
                    if (transactionManager.IsValidSale()) { req.Approve(); }
                    break;
                case SELL_BOARD_PIECE:
                    PieceData sellBoardPieceData = req.GetData() as PieceData;
                    if (transactionManager.CanSellBoardPiece(sellBoardPieceData.player, sellBoardPieceData.piece)) { req.Approve(); }
                    break;
                case UPGRADE_INCOME:
                    UpgradeIncomeData data_100 = req.GetData() as UpgradeIncomeData;
                    if (transactionManager.CanPurchaseIncreasePassiveIncome(data_100.player))
                    {
                        req.Approve();
                    }
                    break;
                case UPGRADE_MARKET_RARITY:
                    UpgradeMarketRarityData data_101 = req.GetData() as UpgradeMarketRarityData;
                    if (transactionManager.CanPurchaseIncreaseMarketRarity(data_101.player))
                    {
                        req.Approve();
                    }
                    break;
                case UPGRADE_MARKET_SIZE:
                    UpgradeMarketSizeData data_102 = req.GetData() as UpgradeMarketSizeData;
                    if (transactionManager.CanPurchaseIncreaseMarketSize(data_102.player))
                    {
                        req.Approve();
                    }
                    break;
                case UPGRADE_ARMY_SIZE:
                    UpgradeArmySizeData data_103 = req.GetData() as UpgradeArmySizeData;
                    if (transactionManager.CanPurchaseIncreaseArmySize(data_103.player))
                    {
                        req.Approve();
                    }
                    break;
                default:
                    Debug.LogErrorFormat("{0}: {1} issued an invalid action code: {2}", CLASS_NAME, req.GetRequester(), req.GetActionType());
                    return req;
            }

            LogValidateRequest(req);
            return req;
        }

        /// <summary>
        /// ExecuteRequest utilises various Managers to execute a request given if it is approved. For all clients.
        /// </summary>
        /// <param name="req"></param>
        public void ExecuteRequest(Request req)
        {
            if (!req.IsApproved())
            {
                LogRejectedRequest(req);
                return;
            }

            switch (req.GetActionType())
            {
                case MOVE_FROM_BENCH_TO_BOARD:
                    PieceMovementData data_0 = (PieceMovementData)req.GetData();
                    arrangementManager.MoveBenchToBoard(data_0.player, data_0.piece, data_0.tile);
                    break;
                case MOVE_FROM_BOARD_TO_BENCH:
                    BoardToBenchData data_1 = (BoardToBenchData)req.GetData();
                    arrangementManager.MoveBoardToBench(data_1.player, data_1.piece, data_1.slotIndex);
                    break;
                case MOVE_ON_BOARD:
                    PieceMovementData data_2= (PieceMovementData)req.GetData();
                    arrangementManager.MovePieceOnBoard(data_2.player, data_2.piece, data_2.tile);
                    break;
                case INIT_PHASE:
                    PhaseManagementData data_10 = req.GetData() as PhaseManagementData;
                    phaseManager.Initialize(data_10.numPlayers);
                    break;
                case ROUND_START:
                    phaseManager.StartRound();
                    break;
                case MARKET_PHASE:
                    MarketManagementData data_11 = (MarketManagementData)req.GetData();
                    marketManager.SetMarketItems(data_11.pieces);
                    phaseManager.StartMarketPhase();
                    break;
                case PRECOMBAT_PHASE:
                    PreCombatData data_12 = req.GetData() as PreCombatData;
                    phaseManager.StartPreCombat(data_12.enemies);
                    break;
                case POSTCOMBAT_PHASE:
                    phaseManager.StartPostCombat();
                    break;
                case BUY_PIECE:
                    PieceTransactionData data_5 = (PieceTransactionData)req.GetData();
                    transactionManager.PurchaseMarketPieceToBench(data_5.player, data_5.piece, data_5.price);
                    break;
                case SELL_PIECE:
                    PieceTransactionData data_6 = (PieceTransactionData)req.GetData();
                    transactionManager.SellBenchPiece(data_6.player, data_6.piece);
                    break;
                case SELL_BOARD_PIECE:
                    PieceData sellBoardPieceData = req.GetData() as PieceData;
                    transactionManager.SellBoardPiece(sellBoardPieceData.player, sellBoardPieceData.piece);
                    break;
                case UPGRADE_INCOME:
                    UpgradeIncomeData data_100 = req.GetData() as UpgradeIncomeData;
                    transactionManager.PurchaseIncreasePassiveIncome(data_100.player);
                    break;
                case UPGRADE_MARKET_RARITY:
                    UpgradeMarketRarityData data_101 = req.GetData() as UpgradeMarketRarityData;
                    transactionManager.PurchaseIncreaseMarketRarity(data_101.player, data_101.pieces);
                    break;
                case UPGRADE_MARKET_SIZE:
                    UpgradeMarketSizeData data_102 = req.GetData() as UpgradeMarketSizeData;
                    transactionManager.PurchaseIncreaseMarketSize(data_102.player, data_102.piece);
                    break;
                case UPGRADE_ARMY_SIZE:
                    UpgradeArmySizeData data_103 = req.GetData() as UpgradeArmySizeData;
                    transactionManager.PurchaseIncreaseArmySize(data_103.player);
                    break;
                default:
                    Debug.LogErrorFormat("{0}: {1} issued request of invalid action type {2}", req.GetRequester(), req.GetActionType());
                    break;
            }
            LogExecuteRequest(req);
        }
        #endregion

        #region Private Methods
        private void LogValidateRequest(Request req)
        {
            if (DEBUG_MODE)
            {
                if (req.IsApproved())
                {
                    Debug.LogFormat("{0}: Request to '{1}' from  {2} is approved", CLASS_NAME, GetActionName(req.GetActionType()), req.GetRequester());
                }
                else
                {
                    Debug.LogFormat("{0}: Request to '{1}' from  {2} is rejected", CLASS_NAME, GetActionName(req.GetActionType()), req.GetRequester());
                }
            }

        }

        private void LogExecuteRequest(Request req)
        {
            if (DEBUG_MODE)
            {
                Debug.LogFormat("{0}: Executing '{1}' by {2}", CLASS_NAME, GetActionName(req.GetActionType()), req.GetRequester());
            }

        }

        private void LogRejectedRequest(Request req)
        {
            if (DEBUG_MODE)
            {
                Debug.LogFormat("{0}: Request to execute '{1}' from {2} is rejected", CLASS_NAME, GetActionName(req.GetActionType()), req.GetRequester());
            }

        }
        #endregion
    }
}
