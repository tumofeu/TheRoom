﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    const float stepLength = 2;

    public float speed;
    public AudioSource footstep;

    private Vector3 previousPosition;
    private float pathLengthFootstep;

    const float verticalLookLimitUp = 90f;
    const float verticalLookLimitDown = -60f;

    float verticalRotation = 0;
    const float sensitivity = 6;

    // Use this for initialization
    void Start()
    {
        pathLengthFootstep = 0;
        previousPosition = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMouseLook();
        ProcessMovement();
    }

    private void ProcessMouseLook()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        var currentUpDown = Camera.main.transform.rotation.eulerAngles;
        verticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalLookLimitDown, verticalLookLimitUp);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void ProcessMovement()
    {
        float forwardSpeed = Input.GetAxis("Vertical") * speed;
        float sideSpeed = Input.GetAxis("Horizontal") * speed;

        Vector3 speedResult = new Vector3(sideSpeed, 0, forwardSpeed);
        speedResult = transform.rotation * speedResult;
        CharacterController cc = GetComponent<CharacterController>();
        cc.SimpleMove(speedResult);

        pathLengthFootstep += (transform.position - previousPosition).magnitude;
        previousPosition = transform.position;

        ProcessFootsteps();
    }

    private void ProcessFootsteps()
    {
        if (pathLengthFootstep >= stepLength)
        {
            footstep.Play();
            pathLengthFootstep = 0;
        }
    }
}
