using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour 
{
    public float playerSpeed = 12;
    public float horizontal = 3; 
    public bool isMoving = false;

    private Rigidbody rb;
    private Vector3 moveInput = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput = Vector3.zero;

        if (Keyboard.current.upArrowKey.isPressed)
            moveInput += Vector3.forward;

        if (Keyboard.current.leftArrowKey.isPressed)
            moveInput += Vector3.left;

        if (Keyboard.current.rightArrowKey.isPressed)
            moveInput += Vector3.right;

        isMoving = moveInput != Vector3.zero;

        if (Keyboard.current.fKey.wasReleasedThisFrame) {  // toggle fast/slow
			if(playerSpeed == 12){
            playerSpeed = 30;
            }
            else {
                playerSpeed = 12;
            }
		}
    }
    void FixedUpdate()
    {
        Vector3 targetPos = rb.position + moveInput.normalized * playerSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector3(targetPos.x, rb.position.y, targetPos.z));
        
        rb.MoveRotation(Quaternion.identity); // Rotation IMMER auf "aufrecht" zwingen
	}
}