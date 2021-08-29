using System;
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

        void Start()
        {
            this._rigidbody2D = GetComponent<Rigidbody2D>();
            this._animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            this.Move();

            this.Jump();
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
            if (Input.GetButtonDown("Jump") && this._jumps < this.limitJumps)
            {
                var newVelocity = new Vector2(this._rigidbody2D.velocity.x, this.velocityJump);

                this._rigidbody2D.velocity = newVelocity;
                
                this._animator.SetBool(AnimationParameter.InFloor, false);

                this._jumps++;
            }
            
            this._animator.SetFloat(AnimationParameter.AirPosition, this._rigidbody2D.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Tags.Floor))
            {
                this._jumps = 0;
                this._animator.SetBool(AnimationParameter.InFloor, true);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Tags.Floor))
            {
                this._animator.SetBool(AnimationParameter.InFloor, false);
            }
        }
    }
}