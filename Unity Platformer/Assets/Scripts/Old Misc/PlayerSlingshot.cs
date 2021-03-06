﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlingshot : MonoBehaviour {

    private bool onGround;
    private float jumpPressure;
    private float minJump;
    private float maxJumpPressure;
    private Rigidbody rbody;

	// Use this for initialization
	void Start ()
    {
        onGround = true;
        jumpPressure = 0f;
        minJump = 2f;
        maxJumpPressure = 5f;
        rbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(onGround)
        {
            //holding jump button
            if(Input.GetButton("Jump"))
            {
                if(jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime * 10f;
                }
                else
                {
                    jumpPressure = maxJumpPressure;
                }
                
            }
            else
            {
                //jump - slingshot 
                if (jumpPressure > 0f)
                {
                    jumpPressure = jumpPressure + minJump;
                    rbody.velocity = new Vector3(jumpPressure*3f, jumpPressure*1.5f, 0f); // Add velocity in X and Y (Height and Distance)
                    jumpPressure = 0f;
                    onGround = false;
                }
            }
        }
	}

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("ground"))
        {
            onGround = true;
        }
    }
}
