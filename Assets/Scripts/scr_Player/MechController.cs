using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : PlayerController
{
    private void OnEnable()
    {
        SetDefaultMechSettings();
    }

    private void SetDefaultMechSettings()
    {
        fallMultiplier = 17;
        jumpForce = 12;

        offsetRadius = -4.5f;
        groundCheckRadius = 1.5f;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;

    }

    protected override void CheckCrouchInput() { }
    protected override void CheckRollInput() { }
}
