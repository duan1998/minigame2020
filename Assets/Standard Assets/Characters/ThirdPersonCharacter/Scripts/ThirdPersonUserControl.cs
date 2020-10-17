using System;
using UnityEngine;

[Flags]public enum BehaviourType
{
    None = 0,
    Idle = 1,
    Walk = 2,
    Jump = 4,
    Climb = 8,    //（持续性动作）
    PickUp = 16,  //（持续性动作）
    PickDown = 32,
}

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    public Vector3 lastFramePosition; //上一帧的位置
    public Vector3 deltaDisplacement;//位移差
    public BoxCollider climbTrigger;
    public LayerMask defaultMask;
      

    private static Collider[] colliders;
    private void Start()
    {
        lastFramePosition = transform.position;
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
        colliders = new Collider[20];

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool climb = false;
   
        Physics.OverlapBoxNonAlloc(climbTrigger.transform.position+climbTrigger.center, climbTrigger.size / 2, colliders, Quaternion.identity, defaultMask, QueryTriggerInteraction.Collide);
        if (colliders[0] != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i]!=null&&colliders[i].CompareTag("Ladder"))
                {
                    climb = true;
                    break;
                }
            }
        }
        
        if (climb && v<0 &&m_Character.IsGround)
        {
            climb = false;
        }
        if (climb)
        {
            m_Move = v * Vector3.up + h * Vector3.right;
        }
        else
        {
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, climb, m_Jump);
        m_Jump = false;
   
        deltaDisplacement = transform.position - lastFramePosition;
        lastFramePosition = transform.position;
    }

    

}

