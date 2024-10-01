using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class GhostScript3rdPerson : MonoBehaviour
    {
        private Animator Anim;
        private CharacterController Ctrl;
        private Vector3 MoveDirection = Vector3.zero;

        // Cache hash values
        private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
        private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
        private static readonly int SurprisedState = Animator.StringToHash("Base Layer.surprised");
        private static readonly int AttackState = Animator.StringToHash("Base Layer.attack_shift");
        private static readonly int DissolveState = Animator.StringToHash("Base Layer.dissolve");
        private static readonly int AttackTag = Animator.StringToHash("Attack");

        // Dissolve effect
        [SerializeField] private SkinnedMeshRenderer[] MeshR;
        private float Dissolve_value = 1;
        private bool DissolveFlg = false;
        private const int maxHP = 3;
        private int HP = maxHP;
        private Text HP_text;

        // Movement parameters
        [SerializeField] private float Speed = 4;
        [SerializeField] private float pushStrength = 2f; // Strength to push objects
        [SerializeField] private float JumpForce = 5f; // Force for jumping
        private bool canJump = true; // Check if the player can jump
        public bool enableJump;
        [SerializeField] private Vector3 respawnPosition = new Vector3(1, 2, 1); // Default respawn position

        // Camera-related variables
        [SerializeField] private Transform cameraTransform; // Camera that follows the player
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 5, -10); // Offset for the camera position
        [SerializeField] private float cameraSmoothSpeed = 0.125f; // Smooth speed for camera movement

        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;

        void Start()
        {
            Anim = this.GetComponent<Animator>();
            Ctrl = this.GetComponent<CharacterController>();
        }

        void Update()
        {
            STATUS();
            GRAVITY();
            Respawn();

            if (!PlayerStatus.ContainsValue(true))
            {
                MOVE();
                PlayerAttack();
                if (enableJump) Jump();
            }
            else if (PlayerStatus.ContainsValue(true))
            {
                int status_name = 0;
                foreach (var i in PlayerStatus)
                {
                    if (i.Value == true)
                    {
                        status_name = i.Key;
                        break;
                    }
                }
                if (status_name == Dissolve)
                {
                    PlayerDissolve();
                }
                else if (status_name == Attack)
                {
                    PlayerAttack();
                }
            }

            // Update camera position every frame
            UpdateCamera();
        }

        //---------------------------------------------------------------------
        // Jump method with camera adjustment
        //---------------------------------------------------------------------
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && CheckGrounded()) // Jump if grounded and spacebar is pressed
            {
                MoveDirection.y = JumpForce; // Apply upward force
                canJump = false; // Disable jumping while airborne

                // Trigger camera update when jumping
                UpdateCamera();
            }
        }

        //---------------------------------------------------------------------
        // Camera follow method with rotation to match player movement
        //---------------------------------------------------------------------
        private void UpdateCamera()
        {
            // Calculate the desired camera position based on the character's position and offset
            Vector3 desiredPosition = this.transform.position + this.transform.TransformDirection(cameraOffset);

            // Smoothly interpolate the camera's position for smoother movement
            Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed);

            // Apply the new position to the camera
            cameraTransform.position = smoothedPosition;

            // Make the camera look at the player, ensuring it stays aligned with the player's movement
            cameraTransform.LookAt(this.transform.position + Vector3.up * 1.5f); // Adjust the look height slightly above the player
        }


        //---------------------------------------------------------------------
        // Character movement in 3rd person view
        //---------------------------------------------------------------------
        private void MOVE()
        {

            // Get input for movement direction
            float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows
            float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down arrows
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            // If there is input, move and rotate the character
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Ctrl.Move(moveDir * Speed * Time.deltaTime);
            }
            else
            {
                // If no input, transition to idle state
                Anim.CrossFade(IdleState, 0.1f, 0, 0);
            }

            KEY_UP(); // Handle key up for stopping movement
        }

        //---------------------------------------------------------------------
        // Gravity handling
        //---------------------------------------------------------------------
        private void GRAVITY()
        {
            if (Ctrl.enabled)
            {
                if (CheckGrounded())
                {
                    canJump = true; // Re-enable jumping when grounded
                    if (MoveDirection.y < -0.1f)
                    {
                        MoveDirection.y = -0.1f;
                    }
                }
                else
                {
                    MoveDirection.y -= 0.1f;
                }
                Ctrl.Move(MoveDirection * Time.deltaTime);
            }
        }

        //---------------------------------------------------------------------
        // Ground check
        //---------------------------------------------------------------------
        private bool CheckGrounded()
        {
            if (Ctrl.isGrounded && Ctrl.enabled)
            {
                return true;
            }
            Ray ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
            float range = 0.2f;
            return Physics.Raycast(ray, range);
        }

        //---------------------------------------------------------------------
        // Collision detection for pushing boxes
        //---------------------------------------------------------------------
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody box = hit.collider.attachedRigidbody;

            // Only push if the object has a Rigidbody and is not kinematic (i.e., movable)
            if (box != null && !box.isKinematic)
            {
                // Calculate push direction in X and Z, ignore Y
                Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z); // Ignore the Y axis

                // Apply force to the box in the horizontal plane
                box.velocity = pushDir * pushStrength; // Apply velocity to the box
            }
        }

        //---------------------------------------------------------------------
        // Respawn method
        //---------------------------------------------------------------------
        private void Respawn()
        {
            if (Input.GetKeyDown(KeyCode.R)) // Changed to R key
            {
                // player HP
                HP = maxHP;

                Ctrl.enabled = false;
                this.transform.position = respawnPosition;
                this.transform.rotation = Quaternion.Euler(Vector3.zero); // player facing
                Ctrl.enabled = true;

                // reset Dissolve
                Dissolve_value = 1;
                for (int i = 0; i < MeshR.Length; i++)
                {
                    MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
                }
                // reset animation
                Anim.CrossFade(IdleState, 0.1f, 0, 0);
            }
        }

        //---------------------------------------------------------------------
        // Character status handling
        //---------------------------------------------------------------------
        private const int Dissolve = 1;
        private const int Attack = 2;
        private const int Surprised = 3;
        private Dictionary<int, bool> PlayerStatus = new Dictionary<int, bool>
        {
            { Dissolve, false },
            { Attack, false },
            { Surprised, false },
        };

        private void STATUS()
        {
            // dissolve
            if (DissolveFlg && HP <= 0)
            {
                PlayerStatus[Dissolve] = true;
            }
            else if (!DissolveFlg)
            {
                PlayerStatus[Dissolve] = false;
            }

            // attack
            if (Anim.GetCurrentAnimatorStateInfo(0).tagHash == AttackTag)
            {
                PlayerStatus[Attack] = true;
            }
            else if (Anim.GetCurrentAnimatorStateInfo(0).tagHash != AttackTag)
            {
                PlayerStatus[Attack] = false;
            }

            // surprised
            if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash == SurprisedState)
            {
                PlayerStatus[Surprised] = true;
            }
            else if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash != SurprisedState)
            {
                PlayerStatus[Surprised] = false;
            }
        }

        //---------------------------------------------------------------------
        // Player attack handling
        //---------------------------------------------------------------------
        private void PlayerAttack()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Anim.CrossFade(AttackState, 0.1f, 0, 0);
            }
        }

        //---------------------------------------------------------------------
        // Handle key release to stop movement
        //---------------------------------------------------------------------
        private void KEY_UP()
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                // Stop movement
                MoveDirection = Vector3.zero;
            }
        }

        //---------------------------------------------------------------------
        // Player dissolve method
        //---------------------------------------------------------------------
        private void PlayerDissolve()
        {
            if (PlayerStatus[Dissolve] && DissolveFlg)
            {
                Anim.CrossFade(DissolveState, 0.1f, 0, 0);
                Dissolve_value = Mathf.Clamp(Dissolve_value - Time.deltaTime, 0, 1);
                for (int i = 0; i < MeshR.Length; i++)
                {
                    MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
                }

                if (Dissolve_value <= 0)
                {
                    DissolveFlg = false;
                }
            }
        }
    }
}
