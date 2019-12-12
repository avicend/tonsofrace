using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    List<CharacterJoint> _ragdollCharacterJointList = new List<CharacterJoint>();

    List<RagdollController> _ragdollControllerList = new List<RagdollController>();

    List<Rigidbody> _ragdollRigidbodyList = new List<Rigidbody>();

    List<Collider> _ragdollColliderList = new List<Collider>();

    RagdollController _tempRagdollController;

    Collider _tempCollider;

    Rigidbody _rigidbody, _tempRigidbody, _spearRigidbody;

    Collider bodyCollider;

    private Animator _characterAnimator;

    
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _characterAnimator = transform.GetComponent<Animator>();
        bodyCollider = transform.GetComponent<Collider>();

        GetComponentsInChildren(_ragdollCharacterJointList);

        for (int i = 0; i < _ragdollCharacterJointList.Count; i++)
        {
            _tempRigidbody = _ragdollCharacterJointList[i].GetComponent<Rigidbody>();
            _ragdollRigidbodyList.Add(_tempRigidbody);

            _tempCollider = _ragdollCharacterJointList[i].GetComponent<Collider>();
            _ragdollColliderList.Add(_tempCollider);

            _tempRagdollController = _ragdollCharacterJointList[i].gameObject.AddComponent<RagdollController>();
            _tempRagdollController.SetRagdoll(_ragdollCharacterJointList[i]);
            _ragdollControllerList.Add(_tempRagdollController);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetActiveRagdollPhysics(bool isActive)
    {
       

        _rigidbody.useGravity = isActive;
        _rigidbody.velocity = Vector3.zero;

        bodyCollider.enabled = !isActive;
        _characterAnimator.enabled = !isActive;
        

       

       

       /* if (isActive)
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
        else
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }*/

        for (int i = 0; i < _ragdollRigidbodyList.Count; i++)
        {
            _ragdollRigidbodyList[i].useGravity = isActive;
            _ragdollRigidbodyList[i].isKinematic = !isActive;
        }

        for (int i = 0; i < _ragdollColliderList.Count; i++)
        {
            _ragdollColliderList[i].enabled = isActive;
        }

        for (int i = 0; i < _ragdollControllerList.Count; i++)
        {
            if (isActive)
            {
                _ragdollControllerList[i].CreateRagdoll();
            }
            else
            {
                _ragdollControllerList[i].DestroyRagdoll();
            }
        }
    }
}
