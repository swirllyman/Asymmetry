using Photon.Pun;
using UnityEngine;

public enum NetworkPlayerType { VR, PC, Both}
public class NetworkPlayerController : MonoBehaviourPun
{
    public NetworkPlayerType myPlayerType;
    public AudioSource speaker;
    public NetworkPlayer[] networkPlayers;

    public void SetupNetworkPlayer(NetworkPlayerType playerType)
    {
        myPlayerType = playerType;
        photonView.RPC(nameof(SetupPlayer_RPC), RpcTarget.AllBuffered, (int)playerType);
    }

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
                    networkPlayers[0].SetupPlayer(LocalPlayerPlatform.singleton.platform_VR, true);
                }
                break;
            case 1:
                networkPlayers[0].HidePlayer();

                speaker.transform.parent = networkPlayers[1].head;
                if (photonView.IsMine)
                {
                    networkPlayers[1].SetupPlayer(LocalPlayerPlatform.singleton.platform_PC, false);
                }
                break;
            case 2:
                speaker.transform.parent = networkPlayers[0].head;
                networkPlayers[0].SetupPlayer(LocalPlayerPlatform.singleton.platform_VR, true);
                networkPlayers[1].SetupPlayer(LocalPlayerPlatform.singleton.platform_PC, false);
                break;
        }
    }
    
}
