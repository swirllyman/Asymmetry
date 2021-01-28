using UnityEngine;
using UnityEngine.XR;
using Cinemachine;

namespace Asymmetry
{
    /// <summary>
    /// Primary Class for handling Asymmetrical Gameplay
    /// </summary>
    public class AsymmetricCrossPlatformController : MonoBehaviour
    {
        public static AsymmetricCrossPlatformController singleton;

        [Header("Place VR Player Platform Here")]
        public GameObject vrPlayerObject;
        public AudioListener vrListener;

        [Header("Place PC Player Platform Here")]
        public GameObject pcPlayerObject;
        public AudioListener pcListener;

        [Header("Desktop View Camera")]
        public GameObject desktopCamera;
        public CinemachineFreeLook cinemachineData;

        [Header("UI")]
        public GameObject[] gameButtons;

        /// <summary>
        /// Used to determine setup for online state
        /// {0 = VR, 1 = PC, 2 = Both (Asymmetric)}
        /// </summary>
        internal int playerType = 0;

        AsymmetricPlayerPlatform platform;

        private void Awake()
        {
            singleton = this;
            platform = GetComponent<AsymmetricPlayerPlatform>();

            //Check if we have a VR device
            if (XRSettings.isDeviceActive)
            {
                SetupVR();
                
                //If VR and android, make sure to disable asymmetric rendering stuff.
                if (Application.platform == RuntimePlatform.Android)
                {
                    desktopCamera.SetActive(false);
                }
            }
            else
            {
                SetupPC();
            }
        }

        //NOTE: Most of the setup calls are tied into Unity Canvas Buttons
        #region Platform Setup

        /// <summary>
        /// Sets up the VR Controller. If a PC player exists at this point they are removed.
        /// </summary>
        public void SetupVR()
        {
            cinemachineData.Follow = platform.platform_VR.head;
            cinemachineData.LookAt = platform.platform_VR.head;
            cinemachineData.m_XAxis.m_MaxSpeed = 0.0f;
            playerType = 0;

            vrListener.enabled = true;
            pcListener.enabled = false;

            vrPlayerObject.SetActive(true);
            pcPlayerObject.SetActive(false);

            if (Application.platform != RuntimePlatform.Android)
            {
                gameButtons[0].SetActive(true);
                gameButtons[1].SetActive(true);
                gameButtons[2].SetActive(false);
                gameButtons[3].SetActive(false);
            }
        }

        /// <summary>
        /// Sets up the PC Controller. If a VR player exists at this point they are removed.
        /// </summary>
        public void SetupPC()
        {
            if (XRSettings.isDeviceActive)
            {
                gameButtons[0].SetActive(false);
                gameButtons[1].SetActive(false);
                gameButtons[2].SetActive(true);
            }
            else
            {
                gameButtons[0].SetActive(false);
                gameButtons[1].SetActive(false);
                gameButtons[2].SetActive(false);
                gameButtons[3].SetActive(false);
            }

            playerType = 1;

            vrListener.enabled = false;
            pcListener.enabled = true;
            vrPlayerObject.SetActive(false);
            pcPlayerObject.SetActive(true);
            
            cinemachineData.m_XAxis.m_MaxSpeed = 300.0f;
            cinemachineData.Follow = platform.platform_PC.head;
            cinemachineData.LookAt = platform.platform_PC.head;
        }

        /// <summary>
        /// Adds in a PC player for Asymmetric gameplay.
        /// </summary>
        public void AddPCPlayer()
        {
            pcPlayerObject.SetActive(true);
            playerType = 2;
            gameButtons[0].SetActive(false);
            gameButtons[3].SetActive(true);

            cinemachineData.m_XAxis.m_MaxSpeed = 300.0f;
            cinemachineData.Follow = platform.platform_PC.head;
            cinemachineData.LookAt = platform.platform_PC.head;
        }
        #endregion
    }
}