using OVRTouchSample;

public class MyHand : Hand
{
    public int handID;

    bool flexing, pinching, pointing, thumbsUpping;

    public delegate void UpdateHandPose(int handID, float flex, float pinch, float point, float thumbsUp);
    public event UpdateHandPose onUpdateHandPose;

    protected override void UpdateAnimStates()
    {
        bool broadcastMajorUpdate = false;
        bool grabbing = m_grabber.grabbedObject != null;
        HandPose grabPose = m_defaultGrabPose;
        if (grabbing)
        {
            HandPose customPose = m_grabber.grabbedObject.GetComponent<HandPose>();
            if (customPose != null) grabPose = customPose;
        }
        // Pose
        HandPoseId handPoseId = grabPose.PoseId;
        m_animator.SetInteger(m_animParamIndexPose, (int)handPoseId);

        // Flex
        // blend between open hand and fully closed fist
        float flex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);
        m_animator.SetFloat(m_animParamIndexFlex, flex);

        // Point
        bool canPoint = !grabbing || grabPose.AllowPointing;
        float point = canPoint ? m_pointBlend : 0.0f;
        //print("Point " +point);
        m_animator.SetLayerWeight(m_animLayerIndexPoint, point);

        // Thumbs up
        bool canThumbsUp = !grabbing || grabPose.AllowThumbsUp;
        float thumbsUp = canThumbsUp ? m_thumbsUpBlend : 0.0f;
        //print("thumbsUp " + thumbsUp);
        m_animator.SetLayerWeight(m_animLayerIndexThumb, thumbsUp);

        float pinch = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller);
        m_animator.SetFloat("Pinch", pinch);


        //Check For Major Changes
        if (thumbsUp >= .95f & !thumbsUpping)
        {
            thumbsUpping = true;
            broadcastMajorUpdate = true;
        }
        else if (thumbsUp < .05f && thumbsUpping)
        {
            thumbsUpping = false;
            broadcastMajorUpdate = true;
        }

        if (point >= .95f & !pointing)
        {
            pointing = true;
            broadcastMajorUpdate = true;
        }
        else if (point < .05f && pointing)
        {
            pointing = false;
            broadcastMajorUpdate = true;
        }

        if (flex >= .95f &! flexing)
        {
            flexing = true;
            broadcastMajorUpdate = true;
        }
        else if(flex < .05f && flexing)
        {
            flexing = false;
            broadcastMajorUpdate = true;
        }

        if(pinch >= .95f & !pinching)
        {
            broadcastMajorUpdate = true;
            pinching = true;
        }
        else if(pinch < .05f && pinching)
        {
            pinching = false;
            broadcastMajorUpdate = true;
        }

        if (broadcastMajorUpdate)
        {
            onUpdateHandPose?.Invoke(handID, flex, pinch, point, thumbsUp);
        }
    }
}
