using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostCharacter : MonoBehaviour
{
    private Animator animator;
    private BehaviorRecord behaviourRecord;
    private bool bPlayingRecord;

    public Transform playerTrans;
    private UnityAction playOverAction;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetBehaviourRecord(BehaviorRecord behaviourRecord,UnityAction action)
    {
        this.behaviourRecord = behaviourRecord;
        this.bPlayingRecord = true;
        frameIdx = 0;
        playOverAction = action;
        transform.position = playerTrans.position;
        transform.rotation = playerTrans.rotation;
    }


    int frameIdx;
    private void FixedUpdate()
    {
        if (bPlayingRecord)
        {
            Behaviour tempBehaviour = behaviourRecord[frameIdx];
            SetTransform(tempBehaviour);
            SetAnimator(tempBehaviour);
            frameIdx++;
            if(frameIdx>=behaviourRecord.FrameCount)
            {
                bPlayingRecord = false;
                animator.speed = 0;
                playOverAction();
            }
        }
    }
    void SetTransform(Behaviour behaviour)
    {
        Vector3 targetPosition = Vector3.zero;
        //targetPosition = playerTrans.forward * behaviour.deltaDisplacement.z + playerTrans.up * behaviour.deltaDisplacement.y + playerTrans.right * behaviour.x;
        targetPosition = transform.position;
        targetPosition.y += behaviour.deltaDisplacement.y;
        transform.position = targetPosition;

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + behaviour.deltaYAxis, transform.eulerAngles.z);
    }
    void SetAnimator(Behaviour behaviour)
    {
        animator.SetFloat("Forward", behaviour.forward, 0.1f, behaviour.deltaTime);
        animator.SetFloat("Turn", behaviour.turn, 0.1f, behaviour.deltaTime);
        animator.SetBool("OnGround", behaviour.onGround);
        animator.SetBool("Climb", behaviour.climb);
        animator.SetFloat("Jump", behaviour.jump);
        animator.SetFloat("JumpLeg", behaviour.jumpLeg);

        animator.speed = behaviour.animatorSpeed;
    }




}
