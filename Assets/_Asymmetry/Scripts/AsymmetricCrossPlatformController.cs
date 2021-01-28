using UnityEngine;
using UnityEngine.XR;
using Cinemachine;
using UnityEngine.XR.Management;

namespace Asymmetry
{
    public class AsymmetricCrossPlatformController : MonoBehaviour
    {
        public static AsymmetricCrossPlatformController singleton;

        public int playerType = 0;

        [Header("Place VR Player Platform Here")]
        public GameObject vrPlayerObject;
        public AudioListener vrListener;

        [Header("Place PC Player Platform Here")]
        public GameObject pcPlayerObject;
        public GameObject playerCamera;
        public AudioListener pcListener;
        public CinemachineFreeLook cinemachine;

        [Header("UI")]
        public GameObject[] gameButtons;

        LocalPlayerPlatform platform;

        private void Awake()
        {
            singleton = this;
            platform = GetComponent<LocalPlayerPlatform>();

            if (XRSettings.isDeviceActive)
            {
                SetupVR();

                if (Application.platform == RuntimePlatform.Android)
                {
                    playerCamera.SetActive(false);
                }
            }
            else
            {
                SetupPC();
            }
        }

        private void Update()
        {
            if (Input.GetButtonDown("Submit") && playerType == 0)
            {
                AddPCPlayer();
            }
        }

        public void SetupVR()
        {
            cinemachine.Follow = platform.platform_VR.head;
            cinemachine.LookAt = platform.platform_VR.head;
            cinemachine.m_XAxis.m_MaxSpeed = 0.0f;
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
            
            cinemachine.m_XAxis.m_MaxSpeed = 300.0f;
            cinemachine.Follow = platform.platform_PC.head;
            cinemachine.LookAt = platform.platform_PC.head;
        }

        public void AddPCPlayer()
        {
            pcPlayerObject.SetActive(true);
            playerType = 2;
            gameButtons[0].SetActive(false);
            gameButtons[3].SetActive(true);

            cinemachine.m_XAxis.m_MaxSpeed = 300.0f;
            cinemachine.Follow = platform.platform_PC.head;
            cinemachine.LookAt = platform.platform_PC.head;
        }
    }
}