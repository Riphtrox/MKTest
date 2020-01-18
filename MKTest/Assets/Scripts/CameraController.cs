using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraLoc;
    public Transform playerLoc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraLoc.position.x != playerLoc.position.x)
        {
            cameraLoc.position = new Vector3(playerLoc.position.x, cameraLoc.position.y, cameraLoc.position.z);
        }
    }
}
