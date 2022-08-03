using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosition : MonoBehaviour
{
    public Transform cameraPosition;

    void Update()
    {
        //Turning the camera with the player
        transform.position = cameraPosition.position;
    }
}
