using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: Set up die state

namespace scr_NPCs.scr_Enemies
{
    public class SpiderBot : Enemy
    {
        private enum Positions
        {
            Ground,
            Ceiling,
            LeftWall,
            RightWall
        };

        private readonly Dictionary<Positions, Vector3> _startingRotations = new Dictionary<Positions, Vector3>()
        {
            {Positions.Ground, new Vector3(0, 0, 0)},
            {Positions.Ceiling, new Vector3(0, 0, 180)},
            {Positions.LeftWall, new Vector3(0, 0, 270)},
            {Positions.RightWall, new Vector3(0, 0, 90)}
        };

        private float _vertSpeed;
        [SerializeField] private Positions startingPosition;
        private Vector3 _startingRotation;


        protected override void Start()
        {
            base.Start();
            SetStartingRotation();
            SetHealth();
           
        }

        private void SetStartingRotation()
        {
            switch (startingPosition)
            {
                case Positions.Ground:
                    _startingRotation = _startingRotations[Positions.Ground];
                    GroundCheckDirection = Vector2.down;
                    break;
                case Positions.Ceiling:
                    _startingRotation = _startingRotations[Positions.Ceiling];
                    GroundCheckDirection = Vector2.up;
                    break;
                case Positions.LeftWall:
                    _startingRotation = _startingRotations[Positions.LeftWall];
                    GroundCheckDirection = Vector2.left;
                    break;
                case Positions.RightWall:
                    _startingRotation = _startingRotations[Positions.RightWall];
                    GroundCheckDirection = Vector2.right;
                    break;
            }

            transform.eulerAngles = _startingRotation;
            SetRotationAndSpeed();
        }

        protected override void SetRotationAndSpeed()
        {
            if ((facingRight && startingPosition == Positions.Ground) ||
                (!facingRight && startingPosition == Positions.Ceiling))
            {
                HorizSpeed = base.speed;
                SetHorizontalRotation();
                SetWallCheck(Vector2.right);
            }
            else if ((!facingRight && startingPosition == Positions.Ground) ||
                     facingRight && startingPosition == Positions.Ceiling)
            {
                HorizSpeed = -speed;
                SetHorizontalRotation();
                SetWallCheck(Vector2.left);
            }
            else if ((facingRight && startingPosition == Positions.RightWall) ||
                     !facingRight && startingPosition == Positions.LeftWall)
            {
                _vertSpeed = speed;
                SetVerticalRotation();
                SetWallCheck(Vector2.up);
            }
            else if ((!facingRight && startingPosition == Positions.RightWall) ||
                     facingRight && startingPosition == Positions.LeftWall)
            {
                _vertSpeed = -speed;
                SetVerticalRotation();
                SetWallCheck(Vector2.down);
            }
        }

        protected override void SetHorizontalRotation()
        {
            if (facingRight)
            {
                transform.eulerAngles = new Vector3(_startingRotation.x, 0, _startingRotation.z);
            }
            else if (!facingRight)
            {
                transform.eulerAngles = new Vector3(_startingRotation.x, 180, _startingRotation.z);
            }
        }

        private void SetVerticalRotation()
        {
            if (facingRight)
            {
                transform.eulerAngles = new Vector3(0, _startingRotation.y, _startingRotation.z);
            }
            else if (!facingRight)
            {
                transform.eulerAngles = new Vector3(180, _startingRotation.y, _startingRotation.z);
            }
        }
        
        protected override IEnumerator Patrol()
        {
            while (CurrentState == State.Patrol)
            {
                var stopChance = Random.Range(0f, 100f);
            
                if (stopChance < chanceToStop && TimeSinceLastIdle > TimeBeforeNextIdle)
                {
                    CurrentState = State.Idle;
                }
                else
                {
                    TimeSinceLastIdle += Time.deltaTime;
                }
            
                CheckForGround();
                CheckForWall();

                Rb.velocity = new Vector2(HorizSpeed, _vertSpeed);

                yield return null;
            }
        }
    }
}
