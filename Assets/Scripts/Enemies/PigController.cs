using Helper;
using UnityEngine;

namespace Enemies
{
    public class PigController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [SerializeField] private float velocity;

        private float _direction;

        [SerializeField] private float minStateTime;
        [SerializeField] private float maxStateTime;

        private float _nextStateChange;

        private Animator _animator;

        private BoxCollider2D _boxCollider2D;

        [SerializeField] private LayerMask layerLevel;
        private float _distanceWallCollision = 1f;

        private bool _alive = true;

        void Start()
        {
            this._rigidbody2D = GetComponent<Rigidbody2D>();
            this._animator = GetComponentInChildren<Animator>();
            this._boxCollider2D = GetComponent<BoxCollider2D>();
        }

        void Update()
        {
            if (!this.IsAlive())
            {
                return;
            }
            
            this.ValidateState();
            this.Move();
        }

        private void FixedUpdate()
        {
            if (!this.IsAlive())
            {
                return;
            }
            
            this.OnWall();
        }

        private void OnWall()
        {
            var isGrounded = this.IsGrounded();

            if (isGrounded)
            {
                var direction = GetDirection();

                this._direction = Mathf.Sign(direction.x) * -1;
            }
        }

        private void ValidateState()
        {
            this._nextStateChange -= Time.deltaTime;

            if (this._nextStateChange > 0)
            {
                return;
            }

            this.RandomDirection();
            this.RandomStateTime();
        }

        private void Move()
        {
            var newVelocity = new Vector2(this._direction * this.velocity, this._rigidbody2D.velocity.y);

            this._rigidbody2D.velocity = newVelocity;

            if (this._direction != 0)
            {
                this.transform.localScale = new Vector3(this._direction * -1, 1f, 1f);
            }

            this._animator.SetBool(AnimationParameter.Moving, newVelocity.x != 0);
        }

        private void RandomDirection()
        {
            // -1 - left
            // 1 - right
            // 0 - stopped
            this._direction = Random.Range(-1, 2);
        }

        private void RandomStateTime()
        {
            this._nextStateChange = Random.Range(this.minStateTime, this.maxStateTime);
        }

        private bool IsGrounded()
        {
            // Dire????o do raycast
            var direction = GetDirection();

            // Checando colis??o com a parede
            bool inWall = Physics2D.Raycast(this._boxCollider2D.bounds.center, direction,
                this._distanceWallCollision, this.layerLevel);

            var color = inWall ? Color.red : Color.green;

            // Desenhando linha com o ch??o
            Debug.DrawRay(this._boxCollider2D.bounds.center, direction * this._distanceWallCollision, color);

            return inWall;
        }

        private Vector2 GetDirection()
        {
            return this._rigidbody2D.velocity.x > 0 ? Vector2.right : Vector2.left;
        }

        public bool IsAlive()
        {
            return this._alive;
        }

        public void Die()
        {
            if (!this.IsAlive())
            {
                return;
            }
            
            this._animator.SetTrigger(AnimationParameter.Hit);
            this._alive = false;
            this._rigidbody2D.velocity = Vector2.zero;
            Destroy(this.gameObject, 5f);
        }
    }
}