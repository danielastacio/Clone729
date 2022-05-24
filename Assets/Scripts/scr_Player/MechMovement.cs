using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMovement : PlayerMovement
{
    private void OnEnable()
    {
        SetDefaultMechSettings();
        MechActivated += ActivateMech;
        MechDeactivated += DeactivateMech;
    }
    private void ActivateMech()
    {
        UnFreezeRigidBody();
        _rb.GetComponent<CapsuleCollider2D>().isTrigger = false;
    }

    private void DeactivateMech()
    {
        FreezeRigidBody();
        _rb.GetComponent<CapsuleCollider2D>().isTrigger = true;
    }
    private void SetDefaultMechSettings()
    {
        fallMultiplier = 17;
        jumpForce = 12;

        offsetRadius = -4.5f;
        groundCheckRadius = 1.5f;

        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 2;

        _rb.GetComponent<CapsuleCollider2D>().isTrigger = true;
    }

    protected override void CheckCrouchInput() { }
    protected override void CheckRollInput() { }
    protected override void OnTriggerEnter2D(Collider2D collider) { }

    protected override void OnTriggerExit2D(Collider2D collider) { }
}
