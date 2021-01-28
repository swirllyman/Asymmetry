using UnityEngine;
using Cinemachine;

namespace Asymmetry
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        public Transform cameraTransform;
        public CinemachineFreeLook freeLookCam;

        public Transform groundCheck;
        public LayerMask groundMask;
        public float gravity = -9.81f;
        public float groundDistance = .4f;
        public float moveSpeed = 6.0f;
        public float turnSmoothTime = .1f;
        public float jumpHeight = 3.0f;

        bool isGrounded = false;
        Vector3 velocity;
        float turnSmoothVelocity;
        CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y <= 0)
            {
                velocity.y = -2f;
            }

            float horizontal = Input.GetAxis("Horizontal_Keyboard");
            float vertical = Input.GetAxis("Vertical_Keyboard");
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            freeLookCam.m_RecenterToTargetHeading.m_enabled = direction.magnitude < .1f;

            if (direction.magnitude >= .1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);

                Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            }


            if(Input.GetButtonDown("Jump_Keyboard") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
