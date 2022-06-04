using UnityEngine;

namespace scr_Player
{
    public class MechController : PlayerController
    {
        protected override void SetRigidbodySettings()
        {            
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 2;

            fallMultiplier = 17;
            jumpForce = 12;

            offsetRadius = base.offsetRadius * 2;
            groundCheckRadius = base.groundCheckRadius * 2;
        }

        protected override void CheckCrouchInput() { }
        protected override void CheckRollInput() { }
    }
}
