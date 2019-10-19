using System.Collections.Generic;

namespace Com.Nextplease.IWT
{
    public static class ActionTypes
    {
        public const int MOVE_FROM_BENCH_TO_BOARD = 0;
        public const int MOVE_FROM_BOARD_TO_BENCH = 1;
        public const int MOVE_ON_BOARD = 2;
        public const int BUY_PIECE = 5;
        public const int SELL_PIECE = 6;
        public const int SELL_BOARD_PIECE = 7;
        public const int INIT_PHASE = 10;
        public const int MARKET_PHASE = 11;
        public const int PRECOMBAT_PHASE = 12;
        public const int POSTCOMBAT_PHASE = 13;
        public const int ROUND_START = 14;
        public const int UPGRADE_MARKET_RARITY = 101;
        public const int UPGRADE_MARKET_SIZE = 102;
        public const int UPGRADE_ARMY_SIZE = 103;


        static Dictionary<int, string> actionCodeToName = new Dictionary<int, string>{
            { MOVE_FROM_BENCH_TO_BOARD, "Move from Bench to Board" },
            { MOVE_FROM_BOARD_TO_BENCH, "Move from Board to Bench" },
            { MOVE_ON_BOARD, "Move on Board" },
            { BUY_PIECE, "Purchase Piece" },
            { SELL_PIECE, "Sell Piece" },
            { SELL_BOARD_PIECE, "Sell Piece On Board" },
            { INIT_PHASE, "Trigger Initialisation Phase" },
            { MARKET_PHASE, "Trigger Market Phase" },
            { PRECOMBAT_PHASE, "Trigger Pre-combat Phase" },
            { POSTCOMBAT_PHASE, "Trigger Post-combat Phase" },
            { ROUND_START, "Trigger Round Start" },
            { UPGRADE_MARKET_RARITY, "Upgrade Market Rarity" },
            { UPGRADE_MARKET_SIZE, "Upgrade Market Size" },
            { UPGRADE_ARMY_SIZE, "Upgrade Army Size" }
        };

        public static string GetActionName(int code)
        {
            return actionCodeToName[code];
        }
    }
}
