using System;
using UnityEngine;


[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    public BoxCollider climbTrigger;
    public LayerMask defaultMask;
    public bool bCarring;
      

    private static Collider[] colliders;
    [SerializeField] BoxCollider frontTrigger;
    [SerializeField] GameObject interactableBox;

    private void Start()
    {
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
        bCarring = false;
    }


    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!bCarring)
            {
                //如果有，拿走
                Physics.OverlapBoxNonAlloc(frontTrigger.transform.position, frontTrigger.size / 2, colliders, Quaternion.identity, defaultMask, QueryTriggerInteraction.Collide);
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
            else
            {
                bCarring = false;
                Vector3 boxPosition = interactableBox.transform.position+transform.forward*0.5f;
                GameObject obj= GameObject.Instantiate(interactableBox, boxPosition, interactableBox.transform.rotation);
                obj.AddComponent<Rigidbody>();
                obj.AddComponent<BoxCollider>();
                interactableBox.SetActive(false);

            }
           
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool climb = false;

        // 拿东西不能爬梯子
        if (!bCarring)
        {
            Physics.OverlapBoxNonAlloc(climbTrigger.transform.position + climbTrigger.center, climbTrigger.size / 2, colliders, Quaternion.identity, defaultMask, QueryTriggerInteraction.Collide);
            if (colliders[0] != null)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] != null && colliders[i].CompareTag("Ladder"))
                    {
                        climb = true;
                        break;
                    }
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
        m_Character.Move(m_Move, climb, m_Jump,bCarring);
        m_Jump = false;
    }

    



}

