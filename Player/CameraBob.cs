using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Camera bob
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script manages the camera animations
 */
[RequireComponent(typeof(FirstPersonController))]
public class CameraBob : MonoBehaviour
{
    public GameObject jumpFallBob;
    public AnimationCurve headBob;
    public float headBobSpeed = 1f;
    public float headBobAmount = 0.1f;
    public float jumpBobAmount = 10f;
    public float jumpBobSpeed = 10f;

    [HideInInspector]
    public float landingBobTime = 0f;

    private FirstPersonController fps;
    private float jumpbob = 0f;
    private float animTime = 0f;

    void Start()
    {
        //Setup
        fps = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        //Calculate bob
        if (landingBobTime > 0f)
        {
            landingBobTime -= Time.deltaTime;
            jumpbob = Mathf.Lerp(jumpbob, jumpBobAmount, jumpBobSpeed * Time.deltaTime * TimeManager.Instance.timeScale);
        }
        switch (fps.playerState)
        {
            case FirstPersonController.PlayerState.jumping:
                jumpbob = Mathf.Lerp(jumpbob, jumpBobAmount, jumpBobSpeed * Time.deltaTime * TimeManager.Instance.timeScale);
                break;
            case FirstPersonController.PlayerState.falling:
                jumpbob = Mathf.Lerp(jumpbob, -jumpBobAmount, jumpBobSpeed / 2 * Time.deltaTime * TimeManager.Instance.timeScale);
                break;
            default:
                jumpbob = Mathf.Lerp(jumpbob, 0f, jumpBobSpeed * Time.deltaTime * TimeManager.Instance.timeScale);
                break;
        }

        //Increase animation time
        animTime += (fps.movement.normalized.magnitude * (headBobSpeed * (Input.GetKey(fps.runKey) ? 1.4f : 1)) * Time.deltaTime * TimeManager.Instance.timeScale); //increase speed based on sprint
        animTime %= 1;

        //Apply bob
        jumpFallBob.transform.localRotation = Quaternion.Euler(new Vector3(jumpbob, 0, 0));
        fps.playerCamera.transform.localPosition = new Vector3(fps.playerCamera.transform.localPosition.x, headBob.Evaluate(animTime) * headBobAmount, fps.playerCamera.transform.localPosition.z);
        fps.playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(fps.rotationY / 10, fps.playerCamera.transform.localRotation.y, headBob.Evaluate(animTime) * headBobAmount) * 10f);
    }
}
