using Door;
using Enemies;
using Helper;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [SerializeField] private float velocity;
        [SerializeField] private float velocityJump;

        private Animator _animator;

        [SerializeField] private int limitJumps;

        private int _jumps;

        private BoxCollider2D _boxCollider2D;

        [SerializeField] private LayerMask layerLevel;
        private float _distanceFloorCollision = 0.5f;

        [SerializeField] private int life = 3;

        [SerializeField] private float invincibilityTime = 2f;

        private float _invincibility;

        private DoorController _doorController;

        private bool _inDoorAction;

        void Start()
        {
            this._rigidbody2D = GetComponent<Rigidbody2D>();
            this._animator = GetComponentInChildren<Animator>();
            this._boxCollider2D = GetComponent<BoxCollider2D>();
        }

        void Update()
        {
            if (!this.IsAlive() || this.IsInDoorAction())
            {
                return;
            }

            this.Move();
            this.Jump();
            this.CheckInvincibility();
            this.EnterDoor();
        }

        private void CheckInvincibility()
        {
            if (!this.IsAlive() || this.IsInDoorAction())
            {
                return;
            }

            if (this.IsInvincible())
            {
                this._invincibility -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            OnFloor();
        }

        private void OnFloor()
        {
            var isGrounded = this.IsGrounded();

            this._animator.SetBool(AnimationParameter.InFloor, isGrounded);

            if (isGrounded)
            {
                this._jumps = 0;
            }
        }

        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal") * this.velocity;
            var newVelocity = new Vector2(horizontal, this._rigidbody2D.velocity.y);

            this._rigidbody2D.velocity = newVelocity;

            this._animator.SetBool(AnimationParameter.Moving, (horizontal != 0));

            if (horizontal != 0)
            {
                this.transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f);
            }
        }

        private void Jump()
        {
            if (this.IsGrounded() && Input.GetButtonDown("Jump") && this._jumps < this.limitJumps)
            {
                this.DoJump();

                // this._animator.SetBool(AnimationParameter.InFloor, false);

                this._jumps++;
            }

            this._animator.SetFloat(AnimationParameter.AirPosition, this._rigidbody2D.velocity.y);
        }

        private void DoJump()
        {
            var newVelocity = new Vector2(this._rigidbody2D.velocity.x, this.velocityJump);

            this._rigidbody2D.velocity = newVelocity;
        }

        private bool IsGrounded()
        {
            // Checando colisão com o chão
            bool inFloor = Physics2D.Raycast(this._boxCollider2D.bounds.center, Vector2.down,
                this._distanceFloorCollision, this.layerLevel);

            var color = inFloor ? Color.red : Color.green;

            // Desenhando linha com o chão
            Debug.DrawRay(this._boxCollider2D.bounds.center, Vector3.down * this._distanceFloorCollision, color);

            return inFloor;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Tags.Floor))
            {
                // this._jumps = 0;
                // this._animator.SetBool(AnimationParameter.InFloor, true);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Tags.Floor))
            {
                // this._animator.SetBool(AnimationParameter.InFloor, false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.EnemiesHit))
            {
                this.CollisionEnemy(other);
                return;
            }

            if (other.CompareTag(Tags.Door))
            {
                this._doorController = other.GetComponent<DoorController>();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Door))
            {
                // this._doorController.Close();
            }
        }

        private void CollisionEnemy(Collider2D other)
        {
            var pigController = other.GetComponentInParent<PigController>();

            if (!pigController.IsAlive() || !this.IsAlive() || this.IsInDoorAction())
            {
                return;
            }

            pigController.Die();

            if (this.transform.position.y > other.transform.position.y)
            {
                this.DoJump();
            }
            else if (this._invincibility <= 0)
            {
                this._invincibility = this.invincibilityTime;
                this.TakeDamage(1);
            }
        }

        private void TakeDamage(int damage)
        {
            this.life -= damage;
            this._animator.SetTrigger(AnimationParameter.Hit);

            if (this.life <= 0)
            {
                this._animator.SetTrigger(AnimationParameter.Dead);
                this._rigidbody2D.velocity = Vector2.zero;
            }
        }

        private bool IsInvincible()
        {
            return this._invincibility > 0;
        }

        public bool IsAlive()
        {
            return this.life > 0;
        }

        private void EnterDoor()
        {
            if (this._doorController && Input.GetKeyDown(KeyCode.W))
            {
                this._doorController.Open();
                this._rigidbody2D.velocity = Vector2.zero;
                this._inDoorAction = true;
                this._animator.SetBool(AnimationParameter.Moving, false);
                Invoke("AnimationEnterDoor", 1f);
                StartCoroutine(this._doorController.GoToDestiny());
            }
        }

        private void AnimationEnterDoor()
        {
            this._animator.SetTrigger(AnimationParameter.EnterDoor);
        }

        private bool IsInDoorAction()
        {
            return this._inDoorAction;
        }
    }
}