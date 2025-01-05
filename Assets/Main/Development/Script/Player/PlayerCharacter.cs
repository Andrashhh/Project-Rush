using KinematicCharacterController;
using UnityEngine;

namespace Root
{
    public enum CrouchInput {
        None, Toggle
    }

    public enum Stance {
        Stand, Crouch, Slide
    }

    public struct CharacterInput {
        public Quaternion Rotation;
        public Vector2 Move;
        public bool Sprint;
        public bool Jump;
        public CrouchInput Crouch;
    }

    public struct CharacterState {
        public bool SlideJump;
        public bool Grounded;
        public Stance Stance;
        public Vector3 Velocity;
        public Vector3 Acceleration;
    }

    public class PlayerCharacter : MonoBehaviour, ICharacterController
    {
        [SerializeField] KinematicCharacterMotor motor;
        [SerializeField] Transform root;
        [SerializeField] Transform cameraTarget;

        [Space]
        [SerializeField] float sprintMultiplier = 1.5f;
        [SerializeField] float walkSpeed = 15f;
        [SerializeField] float crouchSpeed = 7f;
        [SerializeField] float walkResponse = 25f;
        [SerializeField] float crouchResponse = 20f;
        [Space]
        [SerializeField] float airSpeed = 15f;
        [SerializeField] float airAcceleration = 70f;
        [Space]
        [SerializeField] float coyoteTime = 0.1f;
        [SerializeField] float jumpSpeed = 20f;
        [SerializeField] float gravity = -90f;
        [Space]
        [SerializeField] float slideStartSpeed = 25f;
        [SerializeField] float slideEndSpeed = 15f;
        [SerializeField] float slideFriction = .8f;
        [SerializeField] float slideSteerAcceleration = 5f;
        [SerializeField] float slideGravity = -90f;
        [Space]
        [SerializeField] float standHeight = 2f;
        [SerializeField] float crouchHeight = 1f;
        [SerializeField] float crouchHeightResponse = 15f;
        [Range(0, 1)]
        [SerializeField] float cameraStandHeight = .9f;
        [Range(0, 1)]
        [SerializeField] float cameraCrouchHeight = .7f;

        CharacterState state;
        CharacterState tempState;
        CharacterState lastState;

        Quaternion requestedRotation;
        Vector3 requestedMove;
        bool requestedSprint;
        bool requestedJump;
        bool requestedCrouch;
        bool requestedCrouchInAir;

        float timeSinceUngrounded;
        float timeSinceJumpRequested;
        bool ungroundedDueToJump;

        Collider[] uncrouchOverlapResults;

        public void Initialize() {
            SwitchStanceTo(Stance.Stand);
            lastState = state;

            uncrouchOverlapResults = new Collider[8];

            motor.CharacterController = this;
        }

        public void UpdateInput(CharacterInput input) {
            requestedRotation = input.Rotation;

            requestedMove = new Vector3(input.Move.x, 0f, input.Move.y);
            requestedMove = Vector3.ClampMagnitude(requestedMove, 1f);
            requestedMove = input.Rotation * requestedMove;

            requestedSprint = input.Sprint && state.Stance is Stance.Stand;

            var wasRequestingJump = requestedJump;
            requestedJump = requestedJump || input.Jump;
            if(requestedJump && !wasRequestingJump) {
                timeSinceJumpRequested = 0f;
            }

            var wasRequestingCrouch = requestedCrouch;
            requestedCrouch = input.Crouch switch {
                CrouchInput.Toggle => true,
                CrouchInput.None => false,
                _ => requestedCrouch
            };
            if(requestedCrouch && wasRequestingCrouch) {
                requestedCrouchInAir = !state.Grounded;
            }
            else if(!requestedCrouch && wasRequestingCrouch) {
                requestedCrouchInAir = false;
            }
        }

        public void UpdateBody(float deltaTime) {
            var currentHeight = motor.Capsule.height;
            var normalizedHeight = currentHeight / standHeight;
            var cameraTargetHeight = currentHeight * (state.Stance is Stance.Stand ? cameraStandHeight : cameraCrouchHeight);
            var rootTargetScale = new Vector3(1f, normalizedHeight, 1f);

            cameraTarget.localPosition = Vector3.Lerp(cameraTarget.localPosition, new Vector3(0f, cameraTargetHeight, 0f), 1f - Mathf.Exp(-crouchHeightResponse * deltaTime));
            root.localScale = Vector3.Lerp(root.localScale, rootTargetScale, 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)); ;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            state.Acceleration = Vector3.zero;

            if(motor.GroundingStatus.IsStableOnGround) {
                timeSinceUngrounded = 0f;
                ungroundedDueToJump = false;
                state.SlideJump = false;
                var groundedMovement = motor.GetDirectionTangentToSurface(requestedMove, motor.GroundingStatus.GroundNormal) * requestedMove.magnitude;
                {
                    var moving = groundedMovement.sqrMagnitude > 0f;
                    var crouching = state.Stance is Stance.Crouch;
                    var wasStanding = lastState.Stance is Stance.Stand;
                    var wasInAir = !lastState.Grounded;

                    if(moving && crouching && (wasStanding || wasInAir)) {
                        SwitchStanceTo(Stance.Slide);

                        if(wasInAir) {
                            currentVelocity = Vector3.ProjectOnPlane(lastState.Velocity, motor.GroundingStatus.GroundNormal);
                        }

                        var effectiveSlideStartSpeed = slideStartSpeed;
                        if(!lastState.Grounded && !requestedCrouchInAir) {
                            effectiveSlideStartSpeed = 0f;
                            requestedCrouchInAir = false;
                        }
                        var slideSpeed = Mathf.Max(effectiveSlideStartSpeed, currentVelocity.magnitude);
                        currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, motor.GroundingStatus.GroundNormal) * slideSpeed;
                    }
                    if(requestedJump && state.Stance is Stance.Slide) {
                        state.SlideJump = true;
                    }
                }
                if(state.Stance is Stance.Stand or Stance.Crouch) {
                    var speed = state.Stance is Stance.Stand ? (requestedSprint ? walkSpeed * sprintMultiplier : walkSpeed) : crouchSpeed;
                    var response = state.Stance is Stance.Stand ? walkResponse : crouchResponse;

                    var targetVelocity = groundedMovement * speed;
                    var moveVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1f - Mathf.Exp(-response * deltaTime));
                    state.Acceleration = moveVelocity - currentVelocity;
                    currentVelocity = moveVelocity;
                }
                else {
                    currentVelocity -= currentVelocity * (slideFriction * deltaTime);

                    {
                        var force = Vector3.ProjectOnPlane(-motor.CharacterUp, motor.GroundingStatus.GroundNormal) * slideGravity;
                        currentVelocity -= force * deltaTime;
                    }

                    {
                        var currentSpeed = currentVelocity.magnitude;
                        var targetVelocity = groundedMovement * currentSpeed;
                        var steerVelocity = currentVelocity;
                        var steerForce = (targetVelocity - steerVelocity) * slideSteerAcceleration * deltaTime;
                        steerVelocity += steerForce;
                        steerVelocity = Vector3.ClampMagnitude(steerVelocity, currentSpeed);

                        state.Acceleration = (steerVelocity - currentVelocity) / deltaTime;

                        currentVelocity = steerVelocity;
                    }

                    if(currentVelocity.magnitude < slideEndSpeed) {
                        //state.Stance = Stance.Crouch;
                        SwitchStanceTo(Stance.Crouch);
                    }
                }
            }
            else {
                timeSinceUngrounded += deltaTime;
                if(requestedMove.sqrMagnitude > 0f) {
                    var planarMove = Vector3.ProjectOnPlane(requestedMove, motor.CharacterUp).normalized * requestedMove.magnitude;
                    var currentPlanarVelocity = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

                    var moveForce = planarMove * airAcceleration * deltaTime;
                    if(currentPlanarVelocity.magnitude < airSpeed) {
                        var targetPlanarVelocity = currentPlanarVelocity + moveForce;

                        targetPlanarVelocity = Vector3.ClampMagnitude(targetPlanarVelocity, airSpeed);
                        moveForce = targetPlanarVelocity - currentPlanarVelocity;
                    }
                    else if(Vector3.Dot(currentPlanarVelocity, moveForce) > 0f) {
                        var constrainedMoveForce = Vector3.ProjectOnPlane(moveForce, currentPlanarVelocity.normalized);

                        moveForce = constrainedMoveForce;
                    }
                    if(motor.GroundingStatus.FoundAnyGround) {
                        if(Vector3.Dot(moveForce, currentVelocity + moveForce) > 0f) {
                            var obstuctionNormal = Vector3.Cross(motor.CharacterUp, Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal)).normalized;
                            moveForce = Vector3.ProjectOnPlane(moveForce, obstuctionNormal);
                        }
                    }

                    currentVelocity += moveForce;
                }
                currentVelocity += motor.CharacterUp * gravity * deltaTime;
            }

            if(requestedJump) {
                var grounded = motor.GroundingStatus.IsStableOnGround;
                var canCoyoteJump = timeSinceUngrounded < coyoteTime && !ungroundedDueToJump;
                if(grounded || canCoyoteJump) {
                    requestedJump = false;
                    requestedCrouch = false;
                    requestedCrouchInAir = false;

                    motor.ForceUnground(0.1f);
                    ungroundedDueToJump = true;

                    var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
                    var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, jumpSpeed);

                    currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
                }
                else {
                    timeSinceJumpRequested += deltaTime;

                    var canJumpLater = timeSinceJumpRequested < coyoteTime;
                    requestedJump = canJumpLater;
                }
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            var forward = Vector3.ProjectOnPlane(requestedRotation * Vector3.forward, motor.CharacterUp);

            if(forward != Vector3.zero)
                currentRotation = Quaternion.LookRotation(forward, motor.CharacterUp);
        }


        public void BeforeCharacterUpdate(float deltaTime) {
            tempState = state;
            if(requestedCrouch && state.Stance is Stance.Stand) {
                //state.Stance = Stance.Crouch;
                SwitchStanceTo(Stance.Crouch);

                motor.SetCapsuleDimensions(motor.Capsule.radius, crouchHeight, crouchHeight * 0.5f);
            }
        }

        public void AfterCharacterUpdate(float deltaTime) {
            if(!requestedCrouch && state.Stance is not Stance.Stand) {
                motor.SetCapsuleDimensions(motor.Capsule.radius, standHeight, standHeight * 0.5f);

                var pos = motor.TransientPosition;
                var rot = motor.TransientRotation;
                var mask = motor.CollidableLayers;
                if(motor.CharacterOverlap(pos, rot, uncrouchOverlapResults, mask, QueryTriggerInteraction.Ignore) > 0) {
                    requestedCrouch = true;
                    motor.SetCapsuleDimensions(motor.Capsule.radius, crouchHeight, crouchHeight * 0.5f);
                }
                else {
                    SwitchStanceTo(Stance.Stand);
                }
            }

            var totalAcceleration = (state.Velocity - lastState.Velocity) / deltaTime;

            state.Grounded = motor.GroundingStatus.IsStableOnGround;
            state.Velocity = motor.Velocity;
            state.Acceleration = Vector3.ClampMagnitude(state.Acceleration, totalAcceleration.magnitude);
            lastState = tempState;
        }

        public void PostGroundingUpdate(float deltaTime) {
            if(!motor.GroundingStatus.IsStableOnGround && state.Stance is Stance.Slide) {
                //state.Stance = Stance.Crouch;
                SwitchStanceTo(Stance.Crouch);
            }
        }

        public bool IsColliderValidForCollisions(Collider coll) => true;

        public void OnDiscreteCollisionDetected(Collider hitCollider) {
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
            state.Acceleration = Vector3.ProjectOnPlane(state.Acceleration, hitNormal);
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {
        }

        public Transform GetCameraTarget() => cameraTarget;
        public CharacterState GetState() => state;
        public CharacterState GetLastState() => lastState;

        public void SetPosition(Vector3 pos, bool killVelocity = true) {
            motor.SetPosition(pos);
            if(killVelocity) {
                motor.BaseVelocity = Vector3.zero;
            }
        }

        public void SwitchStanceTo(Stance stance) {
            switch(stance) {
                case Stance.Stand:
                    state.Stance = Stance.Stand;
                    break;
                case Stance.Crouch:
                    state.Stance = Stance.Crouch;
                    break;
                case Stance.Slide:
                    state.Stance = Stance.Slide;
                    break;
                default:
                    break;
            }
        }
    }
}
