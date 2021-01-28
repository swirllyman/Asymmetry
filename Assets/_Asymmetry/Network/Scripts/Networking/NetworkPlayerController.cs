using Photon.Pun;
using UnityEngine;

public enum NetworkPlayerType { VR, PC, Both}

/// <summary>
/// Network Controller. Responsible for setting up the player type on the networked on. Allows for Asymmetric players to join a networked lobby.
/// </summary>
public class NetworkPlayerController : MonoBehaviourPun
{
    public NetworkPlayerType myPlayerType;
    public AudioSource speaker;
    public NetworkPlayer[] networkPlayers;

    /// <summary>
    /// Initial Call from Network Manager when a new player first joins the game
    /// </summary>
    /// <param name="playerType">0 = VR, 1 = PC, 2 = Both (Asymmetric)</param>
    public void SetupNetworkPlayer(NetworkPlayerType playerType)
    {
        myPlayerType = playerType;
        photonView.RPC(nameof(SetupPlayer_RPC), RpcTarget.AllBuffered, (int)playerType);
    }

    /// <summary>
    /// Initial RPC Call to determine how to setup the new networked client
    /// </summary>
    /// <param name="playerType"></param>
    [PunRPC]
    void SetupPlayer_RPC(int playerType)
    {
        myPlayerType = (NetworkPlayerType)playerType;
        switch (playerType)
        {
            case 0:
                networkPlayers[1].HidePlayer();
                
                speaker.transform.parent = networkPlayers[0].head;
                if (photonView.IsMine) 
                {
                    networkPlayers[0].SetupPlayer(AsymmetricPlayerPlatform.singleton.platform_VR, true);
                }
                break;
            case 1:
                networkPlayers[0].HidePlayer();

                speaker.transform.parent = networkPlayers[1].head;
                if (photonView.IsMine)
                {
                    networkPlayers[1].SetupPlayer(AsymmetricPlayerPlatform.singleton.platform_PC, false);
                }
                break;
            case 2:
                speaker.transform.parent = networkPlayers[0].head;
                networkPlayers[0].SetupPlayer(AsymmetricPlayerPlatform.singleton.platform_VR, true);
                networkPlayers[1].SetupPlayer(AsymmetricPlayerPlatform.singleton.platform_PC, false);
                break;
        }
    }
    
}
