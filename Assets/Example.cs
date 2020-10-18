using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField]
    public Transform gunObj;

    private void Start()
    {
        Vector3 n1 = new Vector3(1, 1, 0);
        Vector3 n2 = new Vector3(-1, 0, 0);
        print(Vector3.ProjectOnPlane(n2, n1));
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Find the line from the gun to the point that was clicked.
                Vector3 incomingVec = hit.point - gunObj.position;

                // Use the point's normal to calculate the reflection vector.
                Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

                // Draw lines to show the incoming "beam" and the reflection.
                Debug.DrawLine(gunObj.position, hit.point, Color.red);
                Debug.DrawRay(hit.point, reflectVec, Color.green);

            }
        }
    }
}
