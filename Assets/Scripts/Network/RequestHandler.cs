using System.Collections;
using UnityEngine;
using static Com.Nextplease.IWT.ActionTypes;

namespace Com.Nextplease.IWT
{
    public class RequestHandler : MonoBehaviour
    {
        #region Private Fields
        private const string CLASS_NAME = "RequestHandler";
        private const bool DEBUG_MODE = true;
        private int _currentPhaseID = 0;
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
            if (roomManager.IsOffline)
            {
                ExecuteRequest(ValidateRequest(req));
                return;
            }
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
                    PieceMovementData data_0 = req.GetData() as PieceMovementData;
                    if (arrangementManager.CanMoveBenchToBoard(data_0.player, data_0.piece, data_0.tile))
                    {
                        req.Approve();
                    }
                    break;
                case MOVE_FROM_BOARD_TO_BENCH:
                    BoardToBenchData data_1 = req.GetData() as BoardToBenchData;
                    if (arrangementManager.CanMoveBoardToBench(data_1.player, data_1.piece, data_1.slotIndex))
                    {
                        req.Approve();
                    }
                    break;
                case MOVE_ON_BOARD:
                    PieceMovementData data_2 = req.GetData() as PieceMovementData;
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
                case MARKET_PHASE:
                case PRECOMBAT_PHASE:
                case POSTCOMBAT_PHASE:
                    if (roomManager.IsOffline)
                    {
                        req.Approve();
                        break;
                    }

                    phaseManager.SetPlayerReadyForNextPhase(req.GetRequester());
                    if (phaseManager.PlayersReadyForNextPhase())
                    {
                        req.Approve();
                        phaseManager.ClearPlayerReadySet();
                    }
                    break;
                case BUY_PIECE:
                    PieceTransactionData data_5 = req.GetData() as PieceTransactionData;
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
                    PieceMovementData data_0 = req.GetData() as PieceMovementData;
                    arrangementManager.MoveBenchToBoard(data_0.player, data_0.piece, data_0.tile);
                    break;
                case MOVE_FROM_BOARD_TO_BENCH:
                    BoardToBenchData data_1 = req.GetData() as BoardToBenchData;
                    arrangementManager.MoveBoardToBench(data_1.player, data_1.piece, data_1.slotIndex);
                    break;
                case MOVE_ON_BOARD:
                    PieceMovementData data_2 = req.GetData() as PieceMovementData;
                    arrangementManager.MovePieceOnBoard(data_2.player, data_2.piece, data_2.tile);
                    break;
                case INIT_PHASE:
                    InitPhaseData data_10 = req.GetData() as InitPhaseData;
                    phaseManager.Initialize(data_10.numPlayers, data_10.seeds);
                    incrementPhaseID();
                    break;
                case ROUND_START:
                    phaseManager.StartRound();
                    incrementPhaseID();
                    break;
                case MARKET_PHASE:
                    MarketManagementData data_11 = req.GetData() as MarketManagementData;
                    marketManager.SetMarketItems(data_11.pieces);
                    phaseManager.StartMarketPhase();
                    incrementPhaseID();
                    break;
                case PRECOMBAT_PHASE:
                    PreCombatData data_12 = req.GetData() as PreCombatData;
                    phaseManager.randomRoundIndex = data_12.randomIndex;
                    phaseManager.StartPreCombat(data_12.enemies);
                    incrementPhaseID();
                    break;
                case POSTCOMBAT_PHASE:
                    PostCombatData data_13 = req.GetData() as PostCombatData;
                    phaseManager.SetPostCombatData(data_13.health, data_13.gold);
                    phaseManager.StartPostCombat();
                    break;
                case BUY_PIECE:
                    PieceTransactionData data_5 = req.GetData() as PieceTransactionData;
                    transactionManager.PurchaseMarketPieceToBench(data_5.player, data_5.piece, data_5.price);
                    break;
                case SELL_PIECE:
                    PieceTransactionData data_6 = req.GetData() as PieceTransactionData;
                    transactionManager.SellBenchPiece(data_6.player, data_6.piece);
                    break;
                case SELL_BOARD_PIECE:
                    PieceData data_7 = req.GetData() as PieceData;
                    transactionManager.SellBoardPiece(data_7.player, data_7.piece);
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
        private IEnumerator PhaseFailSafe(int phaseID, Request req)
        {
            yield return new WaitForSecondsRealtime(15);
            if (_currentPhaseID == phaseID)
            {
                Debug.LogFormat("{0}: Fail Safe Activated", CLASS_NAME);
                req.Approve();
                this.networkManager.ProcessRequest(req);
            }
        }

        private void incrementPhaseID()
        {
            _currentPhaseID++;
        }

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
