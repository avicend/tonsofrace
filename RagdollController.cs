using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private CharacterJoint _characterJoint;

    private Rigidbody ConnectedBody;
    private Vector3 Anchor;
    private Vector3 Axis;
    private bool AutoConfigureConnectedAnchor;
    //private Vector3 ConnectedAnchor;
    private Vector3 SwingAxis;
    private SoftJointLimit LowTwistLimit;
    private SoftJointLimit HighTwistLimit;
    private SoftJointLimit Swing1Limit;
    private SoftJointLimit Swing2Limit;
    private float BreakForce;
    private float BreakTorque;
    private bool EnableCollision;

    public void SetRagdoll(CharacterJoint characterJoint)
    {
        _characterJoint = characterJoint;

        ConnectedBody = characterJoint.connectedBody;
        Anchor = characterJoint.anchor;
        Axis = characterJoint.axis;
        AutoConfigureConnectedAnchor = characterJoint.autoConfigureConnectedAnchor;
        //ConnectedAnchor = characterJoint.connectedAnchor;
        SwingAxis = characterJoint.swingAxis;
        LowTwistLimit = characterJoint.lowTwistLimit;
        HighTwistLimit = characterJoint.highTwistLimit;
        Swing1Limit = characterJoint.swing1Limit;
        Swing2Limit = characterJoint.swing2Limit;
        BreakForce = characterJoint.breakForce;
        BreakTorque = characterJoint.breakTorque;
        EnableCollision = characterJoint.enableCollision;
    }

    public void CreateRagdoll()
    {
        _characterJoint = gameObject.AddComponent<CharacterJoint>();

        _characterJoint.connectedBody = ConnectedBody;
        _characterJoint.anchor = Anchor;
        _characterJoint.axis = Axis;
        _characterJoint.autoConfigureConnectedAnchor = AutoConfigureConnectedAnchor;
        //_characterJoint.connectedAnchor = ConnectedAnchor;
        _characterJoint.swingAxis = SwingAxis;
        _characterJoint.lowTwistLimit = LowTwistLimit;
        _characterJoint.highTwistLimit = HighTwistLimit;
        _characterJoint.swing1Limit = Swing1Limit;
        _characterJoint.swing2Limit = Swing2Limit;
        _characterJoint.breakForce = BreakForce;
        _characterJoint.breakTorque = BreakTorque;
        _characterJoint.enableCollision = EnableCollision;
    }

    public void DestroyRagdoll()
    {
        Destroy(_characterJoint);
    }
}
