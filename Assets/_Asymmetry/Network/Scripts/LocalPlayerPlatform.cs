using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerPlatform : MonoBehaviour
{
    public static LocalPlayerPlatform singleton;

    public PlayerPlatform platform_VR;
    public PlayerPlatform platform_PC;

    public MyHand[] vrHands;

    public void Awake()
    {
        singleton = this;
        Microphone.Start("", false, 1, 44100);
    }
}

[System.Serializable]
public struct PlayerPlatform
{
    public Transform head, leftHand, rightHand;
}