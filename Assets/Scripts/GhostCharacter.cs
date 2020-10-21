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

    public BoxCollider frontTrigger;

    private bool bCarring;
    private static Collider[] colliders;
    [SerializeField] Transform character;

    [SerializeField] GameObject interactableBox;
    [SerializeField] Transform leftHandTransCarring;
    [SerializeField] Transform rightHandTransCarring;

    [SerializeField] RuntimeAnimatorController backAnimatorController;
    [SerializeField] RuntimeAnimatorController normalAnimatorController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        bCarring = false;
        colliders = new Collider[20];
    }

    public void SetBehaviourRecord(BehaviorRecord behaviourRecord,UnityAction action,bool bBack)
    {
        this.behaviourRecord = behaviourRecord;
        this.bPlayingRecord = true;
        playOverAction = action;
        this.bBack = bBack;
        if (bBack)
            frameIdx = behaviourRecord.FrameCount - 1;
        else
            frameIdx = 0;
        if(bBack)
        {
            if(animator==null)
                animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = backAnimatorController;
        }
        else
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = normalAnimatorController;
        }
 
    }


    int frameIdx;
    bool bBack;
    private void FixedUpdate()
    {
        if (bPlayingRecord)
        {
            Behaviour tempBehaviour = behaviourRecord[frameIdx];
            if(tempBehaviour.carry)
            {
                if(frameIdx==0&&! bBack|| frameIdx == behaviourRecord.FrameCount-1 && bBack)
                {
                    // 谁的手也不可能这么快，第一帧瞬间按上F键 ，所以一定是录制之前已经carry的,就不在检测  
                    bCarring = true;
                }
                else
                {
                    if (!bCarring)
                    {
                        //搬起箱子
                        // 检测一下
                        //如果有，拿走
                        Physics.OverlapBoxNonAlloc(frontTrigger.transform.position, frontTrigger.size / 2, colliders, Quaternion.identity, -1, QueryTriggerInteraction.Collide);
                        if (colliders[0] != null)
                        {
                            for (int i = 0; i < colliders.Length; i++)
                            {
                                if (colliders[i] != null && colliders[i].CompareTag("Box"))
                                {
                                    bCarring = true;
                                    // 隐藏
                                    colliders[i].gameObject.SetActive(false);
                                    Destroy(colliders[i].gameObject);

                                    interactableBox.SetActive(true);
                                    break;
                                }
                            }
                        }
                    }
                    bCarring = true;
                }

                
            }
            else
            {
                if (bCarring)
                {
                    bCarring = false;
                    //放下箱子
                    Vector3 boxPosition = interactableBox.transform.position + transform.forward * 0.5f;
                    GameObject obj = GameObject.Instantiate(interactableBox, boxPosition, interactableBox.transform.rotation);
                    obj.AddComponent<Rigidbody>();
                    obj.AddComponent<BoxCollider>();
                    interactableBox.SetActive(false);
                }
            }


            SetTransform(tempBehaviour);
            SetAnimator(tempBehaviour);

            if(bBack)
            {
                frameIdx--;
                if (frameIdx < 0)
                {
                    bPlayingRecord = false;
                    animator.speed = 0;

                    playOverAction();
                }
            }
            else
            {
                frameIdx++;
                if (frameIdx >= behaviourRecord.FrameCount)
                {
                    bPlayingRecord = false;
                    animator.speed = 0;
                    
                    playOverAction();
                }
            }

        }
    }
    void SetTransform(Behaviour behaviour)
    {
        Vector3 targetPosition = Vector3.zero;
        targetPosition = character.forward * behaviour.deltaDisplacement.z + character.up * behaviour.deltaDisplacement.y + character.right * behaviour.deltaDisplacement.x;
        targetPosition = transform.position+ targetPosition;
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
        animator.SetBool("Carry", behaviour.carry);
        animator.SetBool("ClimbToTop",behaviour.climbToTop);
        animator.speed = behaviour.animatorSpeed;
        
        animator.applyRootMotion = behaviour.applyRootMotion;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (layerIndex == 1)
        {
            if (bCarring)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);

                if (leftHandTransCarring != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTransCarring.position);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTransCarring.position);
                }
            }
        }
    }
    void AnimationClimbToTopStart() { }
    void AnimationClimbToTopEnd() { }

    public void OnAnimatorMove()
    {
        
    }

}
