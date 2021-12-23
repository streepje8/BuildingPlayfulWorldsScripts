using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * First Person Controller
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script controls all the movement behaviour
 */
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    //The player states
    public enum PlayerState
    {
        standing_still,
        moving,
        jumping,
        falling
    }

    public GameObject playerCamera;
    public float sensitifity = 2f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float jumpForce = 20f;
    public float maxJumpTime = 0.5f;
    public float gravity = 9.81f;
    public float gravityMultiplier = 4f;
    public float acceleration = 1f;
    public bool lockCursor = true;
    public bool BlockMovement = false;

    public KeyCode runKey = KeyCode.LeftControl;


    //Readonly variables, should not be accessed from inspector
    [HideInInspector]
    public PlayerState playerState { get; private set; } = PlayerState.standing_still;
    [HideInInspector]
    public Vector3 movement { get; private set; }
    [HideInInspector]
    public float rotationY { get; private set; } = 0;
    [HideInInspector]
    public CharacterController characterController { get; private set; }

    private Vector3 velocity;
    private float moveSpeed = 2f;
    private CameraBob camb;
    private float jumpTime = 0f;
    private bool canJump = true;


    void Start()
    {
        //Setup
        Cursor.lockState = lockCursor ? CursorLockMode.Locked:CursorLockMode.None;
        characterController = GetComponent<CharacterController>();
        camb = GetComponent<CameraBob>();
    }


    void Update()
    {
        //Set movement direction and speed
        if (Input.GetKey(runKey))
        {
            moveSpeed = runSpeed * 4;
        } else
        {
            moveSpeed = walkSpeed * 4;
        }
        Vector3 horizontal = Input.GetAxis("Vertical") * transform.forward;
        Vector3 vertical = Input.GetAxis("Horizontal") * transform.right;
        movement = (horizontal + vertical) * moveSpeed * Time.deltaTime * TimeManager.Instance.timeScale;

        //Calculate jump
        if (characterController.isGrounded)
        {
            canJump = true;
        }
        if (Input.GetButton("Jump") && canJump)
        {
            jumpTime += 1f * Time.deltaTime * TimeManager.Instance.timeScale * TimeManager.Instance.timeScale;
        } else
        {
            jumpTime = 0f;
            canJump = false;
        }
        if(jumpTime > 0 && jumpTime < maxJumpTime)
        {
            velocity.y = Mathf.Lerp(velocity.y, jumpForce * TimeManager.Instance.timeScale * Time.deltaTime,10f * TimeManager.Instance.timeScale * Time.deltaTime);
        }

        //Calculate the player velocity
        velocity.x = Mathf.Lerp(velocity.x, movement.x, acceleration * Time.deltaTime * TimeManager.Instance.timeScale);
        velocity.z = Mathf.Lerp(velocity.z, movement.z, acceleration * Time.deltaTime * TimeManager.Instance.timeScale);
        velocity.y += (new Vector3(0, -gravity, 0) * Time.deltaTime * Time.deltaTime * (gravityMultiplier * 6f)).y * TimeManager.Instance.timeScale;

        //Set current playerstate and apply landingBobTime if a camera bob is present
        playerState = PlayerState.standing_still;
        if (velocity.x * velocity.x > 0.00003 || velocity.z * velocity.z > 0.00003)
        {
            playerState = PlayerState.moving;
        }
        if (characterController.isGrounded && velocity.y < 0)
        {
            if(velocity.y < -0.02f)
            {
                if(camb != null) { 
                    camb.landingBobTime = 0.2f;
                }
            }
            velocity.y = 0;
        } else
        {
            if(velocity.y < -0.005f)
            {
                playerState = PlayerState.falling;
            }
        }
        if (velocity.y > 0)
        {
            playerState = PlayerState.jumping;
            if (camb != null)
            {
                camb.landingBobTime = 0.2f;
            }
        }

        //Calculate rotations on the X axis (Y axis of the mouse because unity.....)
        rotationY += -Input.GetAxis("Mouse Y") * sensitifity;
        rotationY = Mathf.Clamp(rotationY, -80, 80);

        //Apply rotations and transformations
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
        if (camb == null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(rotationY / 10, 0, 0));
        }
        if(MainGameManager.Instance.started || BlockMovement) { 
            characterController.Move(new Vector3(velocity.x, velocity.y, velocity.z));
        }
    }
}
