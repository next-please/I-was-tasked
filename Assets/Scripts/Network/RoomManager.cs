using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


using Photon.Pun;

namespace Com.Nextplease.IWT
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public int NumPlayersToStart = 1; // default set to 1 for single player
        public PhaseManager phaseManager;
        #endregion
        #region Private Serializable Fields
        [SerializeField]
        private Text playerList;
        #endregion

        #region Private Fields
        private bool _offlineMode = false;
        public bool IsOffline { get { return _offlineMode; } }

        private bool _tutorialMode = false;
        public bool IsTutorial { get { return _tutorialMode; } }

        private Dictionary<string, int> _playerMap;
        #endregion

        #region Public Methods
        public void OnClick_LeaveCurrentGame()
        {
            if (IsOffline)
            {
                SceneManager.LoadScene("Lobby");
                return;
            }

            LeaveRoom();
        }

        public void OnClick_QuitGame()
        {
            Application.Quit();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Lobby");
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            if (!PhotonNetwork.IsConnected && NumPlayersToStart == 1)
            {
                _offlineMode = true;
                return;
            }
            _tutorialMode = (bool)PhotonNetwork.CurrentRoom.CustomProperties["isTutorial"];
            NumPlayersToStart = PhotonNetwork.CurrentRoom.MaxPlayers;
            _playerMap = new Dictionary<string, int>();
            UpdatePlayerMap();
            UpdatePlayerList();
        }

        void LoadArena()
        {
            Debug.Log("Load Arena");
            if (!IsOffline)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                }
                Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            }
            UpdatePlayerMap();
            UpdatePlayerList();
        }

        private void UpdatePlayerMap()
        {
            Debug.LogFormat("{0}: Updating Player Map", "RoomManager");
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Photon.Realtime.Player player = PhotonNetwork.PlayerList[i];
                _playerMap.Add(player.UserId, i);
                Debug.LogFormat("{0}: Added {1} to playerMap as index {1}", "RoomManager", player.NickName, i);
            }
            return;
        }

        void UpdatePlayerList()
        {
            if (playerList == null)
                return;

            StringBuilder sb = new StringBuilder("Players: ");
            Debug.Log("UI: Updating Player List...");

            if (IsOffline)
            {
                sb.Append("LocalPlayer");
            }
            else
            {
                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                    sb.Append(player.NickName + ", ");

                sb.Remove(sb.Length - 2, 1);
                playerList.text = sb.ToString();
            }
            playerList.text = sb.ToString();
        }
        #endregion

        #region Public Methods
        public int GetLocalPlayerIndex()
        {
            if (IsOffline)
                return 0;
            return _playerMap[PhotonNetwork.LocalPlayer.NickName];
        }
        public static Player GetLocalPlayer()
        {
            return PhotonPlayerToPlayer(PhotonNetwork.LocalPlayer);
        }

        public static string GetLocalPlayerNickname()
        {
            return PhotonNetwork.LocalPlayer.NickName;
        }

        public static Player PhotonPlayerToPlayer(Photon.Realtime.Player photonPlayer)
        {
            int index = 0;
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                if (p == photonPlayer)
                {
                    return (Player)index;
                }
                index++;
            }
            return Player.Zero; // for debug
        }

        public bool IsRoomFull()
        {
            if (_offlineMode)
            {
                return true;
            }
            return PhotonNetwork.PlayerList.Length == NumPlayersToStart;
        }

        public void SetFullGameMode()
        {
            _tutorialMode = false;
        }
        #endregion

        #region Monobehaviour Methods
        void Start()
        {
            UpdatePlayerList();
            StartGameIfPossible();
        }
        #endregion

        #region Photon Callbacks
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            // remove this for now
            // SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            UpdatePlayerList();
            StartGameIfPossible();
        }


        public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
            UpdatePlayerList();


            // TODO: Remove
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }
        #endregion

        private void StartGameIfPossible()
        {
            if (IsOffline || (PhotonNetwork.PlayerList.Length >= NumPlayersToStart &&
                PhotonNetwork.IsMasterClient))
            {
                phaseManager.StartPhases(NumPlayersToStart);
            }
        }
    }
}