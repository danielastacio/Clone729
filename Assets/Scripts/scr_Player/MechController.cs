using UnityEngine;

namespace scr_Player
{
    public class MechController : PlayerController
    {
        protected override void SetRigidbodySettings()
        {            
            Rb = GetComponent<Rigidbody2D>();
            Rb.gravityScale = 2;

            fallMultiplier = 17;
            jumpForce = 12;

            offsetRadius = base.offsetRadius * 2;
            groundCheckRadius = base.groundCheckRadius * 2;
        }

        protected override void CheckCrouchInput() { }
        protected override void CheckRollInput() { }
    }
}
