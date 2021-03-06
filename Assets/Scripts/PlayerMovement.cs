﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    float horizontalMove =0;
    bool jumping = false;
    bool crouching = false;
    bool grabingP =false;
    bool grabing = false;
    public float runspeed = 40f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runspeed;
        jumping = Input.GetKey("w");
        crouching = Input.GetKey("s");
        grabingP = grabing;
        grabing = Input.GetKey("e");
    }
    void FixedUpdate()
    {
        //movement
        controller.Move(horizontalMove * Time.fixedDeltaTime,crouching, jumping);
        controller.grab(grabingP, grabing);
    }
}
