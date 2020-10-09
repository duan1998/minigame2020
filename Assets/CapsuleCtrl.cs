using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCtrl : MonoBehaviour
{

    private CharacterController characterController;
    public float moveSpeed;
    public float jumpForce;
    public SphereCollider bottomCollider;
    

    private void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float h, v;
    Vector3 deltaMove;

    
    private void Update()
    {
        
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h)>0.005f|| Mathf.Abs(v)>=0.005)
        {
            //移动
            deltaMove.x = h * Time.deltaTime * moveSpeed;
            deltaMove.z = v * Time.deltaTime * moveSpeed;
        }
        if(Input.GetKeyDown(KeyCode.Space)&&IsGround())
        {
            deltaMove.y = jumpForce;
        }

    }
    private void FixedUpdate()
    {
        characterController.Move(deltaMove);
    }


    bool IsGround()
    {
        return Physics.OverlapSphere(bottomCollider.transform.position, bottomCollider.radius, 1 << 3).Length > 0;
    }
}
