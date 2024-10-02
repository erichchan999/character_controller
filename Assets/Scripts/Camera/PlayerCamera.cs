using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private Animator animator; 
    private GameObject playerObject;
    private PlayerManager playerManager;
    
    private GameObject camLeft;
    private GameObject camRight;
    private GameObject camUp;
    private GameObject camDown;
    private CinemachineVirtualCamera camUpVcam;
    private CinemachineVirtualCamera camDownVcam;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        playerManager = playerObject.GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();

        camLeft = GameObject.Find("CM vcam PlayerLeft");
        camRight = GameObject.Find("CM vcam PlayerRight");
        camUp = GameObject.Find("CM vcam PanUpFromPlayer");
        camDown = GameObject.Find("CM vcam PanDownFromPlayer");

        camUpVcam = camUp.GetComponent<CinemachineVirtualCamera>();
        camDownVcam = camDown.GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        UpdateCamUpDownFollower();
    }

    // Cam up and Cam down have to follow the current centre of screen (that is, following either cam left or cam right)
    private void UpdateCamUpDownFollower()
    {
        if (playerManager.IsFacingRight)
        {
            camUpVcam.Follow = camRight.transform;
            camDownVcam.Follow = camRight.transform;
        } else {
            camUpVcam.Follow = camLeft.transform;
            camDownVcam.Follow = camLeft.transform;
        }
    }

    private void LateUpdate() {
        CameraAnimatorParams();
    }

    private void CameraAnimatorParams() {
        animator.SetBool("IsFacingRight", playerManager.IsFacingRight);
        animator.SetBool("IsMoveInputDown", playerManager.IsMoveInputDown);
        animator.SetBool("IsLookDown", playerManager.IsLookDown);
        animator.SetBool("IsLookUp", playerManager.IsLookUp);
    }

    
}
