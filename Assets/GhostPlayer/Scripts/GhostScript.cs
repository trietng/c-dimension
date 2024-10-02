using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class GhostScript : MonoBehaviour
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
        // dissolve
        [SerializeField] private SkinnedMeshRenderer[] MeshR;
        private float Dissolve_value = 1;
        private bool DissolveFlg = false;
        private const int maxHP = 3;
        private int HP = maxHP;
        private Text HP_text;

        // moving speed
        [SerializeField] private float Speed = 4;
        [SerializeField] private float pushStrength = 2f; // Strength to push the box
        [SerializeField] private float JumpForce = 5f; // Force for jumping
        private bool canJump = true; // Check if the player can jump

        public bool enableJump;
        public bool enable3rdPerson;

        [SerializeField] private Vector3 respawnPosition = new Vector3(1, 2, 1); // Default respawn position

        // Camera-related variables
        [SerializeField] private Transform cameraTransform; // Camera that follows the player
        [SerializeField] private float cameraSmoothSpeed = 0.125f; // Smooth speed for camera movement

        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;

        void Start()
        {
            Anim = this.GetComponent<Animator>();
            Ctrl = this.GetComponent<CharacterController>();
            //HP_text = GameObject.Find("Canvas/HP").GetComponent<Text>();
            //HP_text.text = "HP " + HP.ToString();
        }

        void Update()
        {
            STATUS();
            GRAVITY();
            Respawn();
            // this character status
            if (!PlayerStatus.ContainsValue(true))
            {
                if (!enable3rdPerson) MOVE();
                else MOVE_3rd();
                PlayerAttack();
                //Damage();
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
                else if (status_name == Surprised)
                {
                    // nothing method
                }
            }
            // Dissolve
            if (HP <= 0 && !DissolveFlg)
            {
                Anim.CrossFade(DissolveState, 0.1f, 0, 0);
                DissolveFlg = true;
            }
            // processing at respawn
            else if (HP == maxHP && DissolveFlg)
            {
                DissolveFlg = false;
            }
        }
        //---------------------------------------------------------------------
        // Jump method using the spacebar
        //---------------------------------------------------------------------
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && CheckGrounded()) // Jump if grounded and spacebar is pressed
            {
                MoveDirection.y = JumpForce; // Apply upward force
                canJump = false; // Disable jumping while airborne
            }
        }
        //---------------------------------------------------------------------
        // character status
        //---------------------------------------------------------------------
        private const int Dissolve = 1;
        private const int Attack = 2;
        private const int Surprised = 3;
        private Dictionary<int, bool> PlayerStatus = new Dictionary<int, bool>
        {
            {Dissolve, false },
            {Attack, false },
            {Surprised, false },
        };
        //------------------------------
        private void STATUS()
        {
            // during dissolve
            if (DissolveFlg && HP <= 0)
            {
                PlayerStatus[Dissolve] = true;
            }
            else if (!DissolveFlg)
            {
                PlayerStatus[Dissolve] = false;
            }
            // during attacking
            if (Anim.GetCurrentAnimatorStateInfo(0).tagHash == AttackTag)
            {
                PlayerStatus[Attack] = true;
            }
            else if (Anim.GetCurrentAnimatorStateInfo(0).tagHash != AttackTag)
            {
                PlayerStatus[Attack] = false;
            }
            // during damaging
            if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash == SurprisedState)
            {
                PlayerStatus[Surprised] = true;
            }
            else if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash != SurprisedState)
            {
                PlayerStatus[Surprised] = false;
            }
        }
        // dissolve shading
        private void PlayerDissolve()
        {
            Dissolve_value -= Time.deltaTime;
            for (int i = 0; i < MeshR.Length; i++)
            {
                MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
            }
            if (Dissolve_value <= 0)
            {
                Ctrl.enabled = false;
            }
        }
        // play a animation of Attack
        private void PlayerAttack()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Anim.CrossFade(AttackState, 0.1f, 0, 0);
            }
        }
        //---------------------------------------------------------------------
        // gravity for fall of this character
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
        // whether it is grounded
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
        // for character moving
        //---------------------------------------------------------------------
        private void MOVE()
        {
            // velocity
            if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash == MoveState)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    MOVE_Velocity(new Vector3(0, 0, Speed), new Vector3(0, 0, 0));
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    MOVE_Velocity(new Vector3(0, 0, -Speed), new Vector3(0, 180, 0));
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    MOVE_Velocity(new Vector3(-Speed, 0, 0), new Vector3(0, -90, 0));
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    MOVE_Velocity(new Vector3(Speed, 0, 0), new Vector3(0, 90, 0));
                }
            }
            KEY_DOWN();
            KEY_UP();
        }

        private void MOVE_3rd()
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
        // value for moving
        //---------------------------------------------------------------------
        private void MOVE_Velocity(Vector3 velocity, Vector3 rot)
        {
            MoveDirection = new Vector3(velocity.x, MoveDirection.y, velocity.z);
            if (Ctrl.enabled)
            {
                Ctrl.Move(MoveDirection * Time.deltaTime);
            }
            MoveDirection.x = 0;
            MoveDirection.z = 0;
            this.transform.rotation = Quaternion.Euler(rot);
        }
        //---------------------------------------------------------------------
        // whether WASD key is key down
        //---------------------------------------------------------------------
        private void KEY_DOWN()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                Anim.CrossFade(MoveState, 0.1f, 0, 0);
            }
        }
        //---------------------------------------------------------------------
        // whether WASD key is key up
        //---------------------------------------------------------------------
        private void KEY_UP()
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    Anim.CrossFade(IdleState, 0.1f, 0, 0);
                }
            }
        }

        //---------------------------------------------------------------------
        // collision detection and pushing mechanics for box
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
        // respawn
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
    }
}
