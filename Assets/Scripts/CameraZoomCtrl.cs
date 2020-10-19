using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomCtrl : MonoBehaviour
{
    public Vector3 minZoomVector;
    public Vector3 maxZoomVector;
    // Start is called before the first frame update

    private Vector3 curZoomVector;

    public float zoomSpeed;
    private Cinemachine.CinemachineFreeLook freeLookCamera;

    private void Awake()
    {
        freeLookCamera = GetComponent<Cinemachine.CinemachineFreeLook>();
    }
    private void Start()
    {
        curZoomVector = new Vector3(freeLookCamera.m_Orbits[0].m_Radius, freeLookCamera.m_Orbits[1].m_Radius, freeLookCamera.m_Orbits[2].m_Radius);
    }
    // Update is called once per frame
    void Update()
    {
        //Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            curZoomVector = Vector3.Lerp(curZoomVector, maxZoomVector, zoomSpeed * Time.deltaTime);
            if (curZoomVector.x > maxZoomVector.x)
                curZoomVector.x = maxZoomVector.x;
            if (curZoomVector.y > maxZoomVector.y)
                curZoomVector.y = maxZoomVector.y;
            if (curZoomVector.z > maxZoomVector.z)
                curZoomVector.z = maxZoomVector.z;
            freeLookCamera.m_Orbits[0].m_Radius = curZoomVector.x;
            freeLookCamera.m_Orbits[1].m_Radius = curZoomVector.y;
            freeLookCamera.m_Orbits[2].m_Radius = curZoomVector.z;
        }
        //Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            curZoomVector = Vector3.Lerp(curZoomVector, minZoomVector, zoomSpeed * Time.deltaTime);
            if (curZoomVector.x < minZoomVector.x)
                curZoomVector.x = minZoomVector.x;
            if (curZoomVector.y < minZoomVector.y)
                curZoomVector.y = minZoomVector.y;
            if (curZoomVector.z < minZoomVector.z)
                curZoomVector.z = minZoomVector.z;
            freeLookCamera.m_Orbits[0].m_Radius = curZoomVector.x;
            freeLookCamera.m_Orbits[1].m_Radius = curZoomVector.y;
            freeLookCamera.m_Orbits[2].m_Radius = curZoomVector.z;
        }

    }
}
