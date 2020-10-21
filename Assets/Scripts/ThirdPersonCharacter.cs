using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_JumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;
    [SerializeField] float m_climbSpeed=2;
    [SerializeField] SphereCollider bottomTrigger;
    [SerializeField] Transform leftHandTransCarring;
    [SerializeField] Transform rightHandTransCarring;

    Rigidbody m_Rigidbody;
    [HideInInspector]
    public Animator m_Animator;
    bool m_IsGrounded;

    private Behaviour tempBehaviour;

    public bool IsGround
    {
        get { return m_IsGrounded; }
    }
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    CapsuleCollider m_Capsule;

    public bool bCarryIKActive;
    public Vector3 lastFramePosition; //上一帧的位置

    private static Collider[] colliders;
    private static RaycastHit[] hits;

    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] bool applyMotion;
    [SerializeField] float animationSpeed;
    [SerializeField] Vector3 rigidbodyVeocity;
    private void Update()
    {
        applyMotion = m_Animator.applyRootMotion;
        animationSpeed = m_Animator.speed;
        rigidbodyVeocity = m_Rigidbody.velocity;
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        colliders = new Collider[20];
        hits = new RaycastHit[20];
        bCarryIKActive = false;
        lastFramePosition=Vector3.zero;


    }

    public void Move(Vector3 move, bool climb, bool jump,bool carry)
    {
        //录制准备
        if (RecordManager.Instance.bRecording&&tempBehaviour==null)
        {
            tempBehaviour = new Behaviour();
        }
        
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();


        if (!climb)
        {
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            m_ForwardAmount = move.z;
            ApplyExtraTurnRotation();
        }
        else
        {
            m_ForwardAmount = 0;
            m_TurnAmount = 0;
        }

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbToTop"))
        {
            if(m_Rigidbody.useGravity)
                m_Rigidbody.useGravity = false;
            if (m_Animator.speed != 1)
            {
                m_Animator.speed = 1;
                RecordFloatData("animatorSpeed", m_Animator.speed);
            }
        }
        else if(climb)
        {
            HandleClimbMovement(move);
        }
        // control and velocity handling is different when grounded and airborne:
        else if (m_IsGrounded)
        {
            HandleGroundedMovement(jump);
        }
        else
        {
            HandleAirborneMovement();
        }

        // send input and other state parameters to the animator
        UpdateAnimator(move,climb, carry);



        RecordFloatData("deltaTime", Time.deltaTime);
        Vector2 tempVector2 = new Vector2(transform.position.x - lastFramePosition.x, transform.position.z - lastFramePosition.z);
        RecordFloatData("deltaHorizontalDisplacement", tempVector2.magnitude);
        RecordFloatData("deltaVerticalDisplacement", transform.position.y - lastFramePosition.y);

        if (tempBehaviour != null)
        {
            RecordManager.Instance.curRecordingRecord.AddBehaviour(tempBehaviour);
            tempBehaviour = null;
        }
        lastFramePosition = transform.position;
    }







    void UpdateAnimator(Vector3 move,bool climb,bool carry)
    {
        m_Animator.SetBool("Carry", carry);

        RecordBoolData("carry", carry);
        if (carry)
        {
            bCarryIKActive = true;
        }
        else
        {
            bCarryIKActive = false;
        }

        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        RecordFloatData("forward", m_ForwardAmount);

        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        RecordFloatData("turn", m_TurnAmount);

        m_Animator.SetBool("OnGround", m_IsGrounded);
        RecordBoolData("onGround", m_IsGrounded);


        

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbToTop"))
        {
            if (m_Animator.speed != 1)
            {
                m_Animator.speed = 1;
                RecordFloatData("animatorSpeed", m_Animator.speed);
            }
            if (m_Animator.GetBool("Climb"))
            {
                m_Animator.SetBool("Climb", false);
                RecordBoolData("climb", false);
            }
        }
        else if(climb)
        {
            m_Animator.SetBool("Climb", true);
            RecordBoolData("climb", true);
            if (Mathf.Abs(move.y) > 0)
            {
                m_Animator.speed = 1f;
                RecordFloatData("animatorSpeed", m_Animator.speed);
            }
            else
            {
                m_Animator.speed = 0;
                RecordFloatData("animatorSpeed", m_Animator.speed);
            }
        }
        else
        {
            if (!m_IsGrounded)
            {
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
                RecordFloatData("jump", m_Rigidbody.velocity.y);
            }
            else
            {
                m_Animator.SetFloat("Jump", 0f);
                RecordFloatData("jump", 0f);
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
                RecordFloatData("jump", m_Rigidbody.velocity.y);
            }
            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
            if (m_IsGrounded)
            {
                m_Animator.SetFloat("JumpLeg", jumpLeg);
                RecordFloatData("jumpLeg", jumpLeg);

            }
            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
                RecordFloatData("animatorSpeed", m_Animator.speed);
            }
            else
            {
                // don't use that while airborne
                m_Animator.speed = 1;
                RecordFloatData("animatorSpeed", m_Animator.speed);
            }
        }

        if(!climb)
        {
            m_Animator.SetBool("Climb", false);
            RecordBoolData("climb", false);
        }
    }

    void HandleAirborneMovement()
    {
        m_Rigidbody.useGravity = true;
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y <= 0 ? m_OrigGroundCheckDistance : 0.01f;
    }


    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            RecordBoolData("applyRootMotion", false);
            m_GroundCheckDistance = 0.1f;
        }
        m_Rigidbody.useGravity = true;
    }

    void HandleClimbMovement(Vector3 move)
    {

        if (m_Rigidbody.useGravity)
            m_Rigidbody.useGravity = false;
        m_Rigidbody.velocity = Vector3.zero;
        // 不能使用
        m_Animator.applyRootMotion = false;
        RecordBoolData("applyRootMotion", false);


        transform.position += move * m_climbSpeed * Time.fixedDeltaTime;

        Physics.OverlapSphereNonAlloc(bottomTrigger.transform.position, bottomTrigger.radius, colliders, -1, QueryTriggerInteraction.Collide);
        if (colliders[0] != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null && colliders[i].gameObject.name == "Top")
                {
                    if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbToTop"))
                    {
                        m_Animator.SetBool("ClimbToTop",true);
                        RecordBoolData("climbToTop", true);
                        // 到顶了
                        m_Animator.SetBool("Climb", false);
                        RecordBoolData("climb", false);
                    }
                    break;
                }
            }
        }    
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        float delatYAxis = m_TurnAmount * turnSpeed * Time.deltaTime;
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

        RecordFloatData("deltaYAxis", delatYAxis);
    }


    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
        if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbToTop")&&Time.deltaTime>0)
        {
            transform.position += m_Animator.deltaPosition;
            m_Rigidbody.velocity = Vector3.zero;
        }
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance),Color.red,10,true);
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;
            RecordBoolData("applyRootMotion", true);
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;

            m_Animator.applyRootMotion = false;
            RecordBoolData("applyRootMotion", false);
        }
    }


    void RecordVectorData(string name,Vector3 param)
    {
        if (tempBehaviour == null)
            return;
        switch (name)
        {
         
        }
        
    }
    void RecordBoolData(string name,bool param)
    {
        if (tempBehaviour == null)
            return;
        switch (name)
        {
            case "onGround":
                tempBehaviour.onGround = param;
                break;
            case "climb":
                tempBehaviour.climb = param;
                break;
            case "climbToTop":
                tempBehaviour.climbToTop = param;
                break;
            case "carry":
                tempBehaviour.carry = param;
                break;
            case "applyRootMotion":
                tempBehaviour.applyRootMotion = param;
                break;
        }
    }
    void RecordFloatData(string name ,float param)
    {
        if (tempBehaviour == null)
            return;

        switch (name)
        {
            case "deltaYAxis":
                tempBehaviour.deltaYAxis = param;
                break;
            case "forward":
                tempBehaviour.forward = param;
                break;
            case "turn":
                tempBehaviour.turn = param;
                break;
            case "jump":
                tempBehaviour.jump = param;
                break;
            case "jumpLeg":
                tempBehaviour.jumpLeg = param;
                break;
            case "deltaHorizontalDisplacement":
                tempBehaviour.deltaHorizontalDisplacement = param;
                break;
            case "deltaVerticalDisplacement":
                tempBehaviour.deltaVerticalDisplacement = param;
                break;
        }

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(layerIndex==1)
        {
            if (bCarryIKActive)
            {
                m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);

                if (leftHandTransCarring != null)
                {
                    m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTransCarring.position);
                    m_Animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTransCarring.position);
                }
            }
        }
    }


    void AnimationClimbToTopEnd()
    {
        m_Animator.SetBool("ClimbToTop",false);
        RecordBoolData("climbToTop", false);
    }


}

