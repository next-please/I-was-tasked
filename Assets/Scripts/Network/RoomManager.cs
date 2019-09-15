using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;


namespace Com.Nextplease.IWT
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public int NumPlayersToStart = 1;
        public PhaseManager phaseManager;
        #endregion
        #region Private Serializable Fields
        [SerializeField]
        private Text playerList;
        #endregion

        #region Public Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Main Scene");
            UpdatePlayerList();
        }

        void UpdatePlayerList()
        {
            if (playerList == null)
            {
                // Debug.Log("UI: Player List is not set. Ignoring UpdatePlayerList().");
                return;
            }

            Debug.Log("UI: Updating Player List...");

            StringBuilder sb = new StringBuilder("Players: ");
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                sb.Append(player.NickName + ", ");
            }

            sb.Remove(sb.Length - 2, 1);

            // Debug.Log("UI: Player list: " + sb.ToString());
            playerList.text = sb.ToString();
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
                    return (Player) index;
                }
                index++;
            }
            return Player.Zero; // for debug
        }

        public bool IsRoomFull()
        {
            return PhotonNetwork.PlayerList.Length == NumPlayersToStart;
        }
        #endregion

        #region Monobehaviour Methods
        void Start()
        {
            UpdatePlayerList();
        }
        #endregion

        #region Photon Callbacks
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            UpdatePlayerList();
            if (PhotonNetwork.PlayerList.Length >= NumPlayersToStart &&
                PhotonNetwork.IsMasterClient)
            {
                phaseManager.StartPhases(NumPlayersToStart);
            }
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
    }
}