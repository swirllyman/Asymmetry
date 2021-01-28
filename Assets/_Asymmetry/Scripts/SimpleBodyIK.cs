using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBodyIK : MonoBehaviour
{
    public Transform followTarget;
    public Transform myBody;
    public Transform myHead;

    [Range(0, 360.0f)]
    public float rotateThreshold = 10.0f;
    public float rotateSpeed = 3.0f;

    [Header("Body Fake Rotation")]
    public Transform bodyOffset;
    public Transform standardTarget;
    public Transform downwardTarget;
    public float maxRotationThreshold = 75.0f;
    public bool updateBodyPostitionBasedOnRotation = false;

    Transform destination;
    Transform myTransform;

    float destinationY;

    private void Start()
    {
        destination = followTarget;
        destinationY = destination.eulerAngles.y;
        myTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!destination) return;

        myTransform.position = destination.position;

        myHead.rotation = destination.rotation;
        myBody.rotation = Quaternion.Slerp(myBody.rotation, Quaternion.Euler(new Vector3(0, destinationY, 0)), Time.deltaTime * rotateSpeed);

        float diff = Mathf.Abs(myBody.eulerAngles.y - destination.eulerAngles.y);
        if (diff >= rotateThreshold && Vector3.Dot(followTarget.up, Vector3.up) > .2f)
        {
            destinationY = destination.transform.eulerAngles.y;
        }

        if (updateBodyPostitionBasedOnRotation)
        {
            float headRotX = myHead.localEulerAngles.x;
            if (headRotX > 0 && headRotX < 100.0f)
            {
                float rotationAmount = myHead.localEulerAngles.x / maxRotationThreshold;
                rotationAmount = Mathf.Clamp01(rotationAmount);
                bodyOffset.transform.localPosition = Vector3.Lerp(standardTarget.localPosition, downwardTarget.localPosition, rotationAmount);
                bodyOffset.transform.localRotation = Quaternion.Slerp(standardTarget.localRotation, downwardTarget.localRotation, rotationAmount);
            }
            else
            {
                bodyOffset.transform.localPosition = Vector3.Lerp(bodyOffset.transform.localPosition, standardTarget.localPosition, Time.deltaTime * 5);
                bodyOffset.transform.localRotation = Quaternion.Slerp(bodyOffset.transform.localRotation, standardTarget.localRotation, Time.deltaTime * 5);
            }
        }
    }
}

