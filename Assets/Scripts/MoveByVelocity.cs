using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveByVelocity : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 200f;
    public float jumpCooldown = 1f;
    private bool canJump = true;
    public Joystick joystick;

    [Header("Physics")]
    public Rigidbody rb;

    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        InputManager.Instance.RegisterOnJumpAction(Jump, true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = InputManager.Instance.movementInput;
        //rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
        rb.velocity = new Vector3(joystick.Direction.x * moveSpeed, rb.velocity.y, joystick.Direction.y * moveSpeed);
    }

    private void OnDestroy()
    {
        InputManager.Instance.RegisterOnJumpAction(Jump, false);
    }

    private IEnumerator JumpCooldown() {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    { 
        if (canJump)
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpForce);
            StartCoroutine(JumpCooldown());
        }
    }

}
