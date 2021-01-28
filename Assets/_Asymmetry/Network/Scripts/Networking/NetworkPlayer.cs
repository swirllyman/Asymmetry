using UnityEngine;
using Photon.Pun;

/// <summary>
/// Basic Class For Handling Network Player Functionality
/// </summary>
public class NetworkPlayer : MonoBehaviourPun
{
    public SimpleBodyIK myIK;
    public Transform head, leftHand, rightHand;
    public Animator[] handAnims;

    AsymmetricPlatform followPlatform;

    private void Start()
    {
        // Disable visuals for local player
        if (photonView.IsMine)
        {
            myIK.gameObject.SetActive(false);
            handAnims[0].gameObject.SetActive(false);
            handAnims[1].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Update positions on local player only
        // PhotonNetworkTransforms update this position to all other players
        if (photonView.IsMine)
        {
            head.position = followPlatform.head.position;
            head.rotation = followPlatform.head.rotation;

            leftHand.position = followPlatform.leftHand.position;
            leftHand.rotation = followPlatform.leftHand.rotation;

            rightHand.position = followPlatform.rightHand.position;
            rightHand.rotation = followPlatform.rightHand.rotation;
        }
    }

    public void HidePlayer()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Setup a new Networked Player
    /// </summary>
    /// <param name="playerPlatform"> Platform used for getting local player position </param>
    /// <param name="usingVR"></param>
    public void SetupPlayer(AsymmetricPlatform playerPlatform, bool usingVR)
    {
        if (photonView.IsMine)
        {
            followPlatform = playerPlatform;
            if (usingVR)
            {
                AsymmetricPlayerPlatform.singleton.vrHands[0].onUpdateHandPose += NetworkPlayer_onUpdateHandPose;
                AsymmetricPlayerPlatform.singleton.vrHands[1].onUpdateHandPose += NetworkPlayer_onUpdateHandPose;
            }
            else
            {
                photonView.RPC(nameof(SetupPlayerPC_RPC), RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void SetupPlayerPC_RPC()
    {
        handAnims[0].gameObject.SetActive(false);
        handAnims[1].gameObject.SetActive(false);
    }

    private void NetworkPlayer_onUpdateHandPose(int handID, float flex, float pinch, float point, float thumbsUp)
    {
        photonView.RPC(nameof(UpdateHandsPose_RPC), RpcTarget.All, handID, flex, pinch, point, thumbsUp);
    }

    [PunRPC]
    public void UpdateHandsPose_RPC(int handID, float flex, float pinch, float point, float thumbsUp)
    {
        handAnims[handID].SetFloat("Pinch", pinch);
        handAnims[handID].SetFloat("Flex", flex);
        handAnims[handID].SetLayerWeight(2, point);
        handAnims[handID].SetLayerWeight(1, thumbsUp);
    }
}
