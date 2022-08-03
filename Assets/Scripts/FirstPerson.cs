using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FirstPerson : MonoBehaviour
{

    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        //Hiding the cursor and setting the mouse sensitivity to 100 when the game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensX = 100f;
        sensY = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse Inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        //Camera FOV zoom out effect
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        //Camera tilt effect
        transform.DOLocalRotate(new Vector3(0,0,zTilt), 0.25f);
    }
}
