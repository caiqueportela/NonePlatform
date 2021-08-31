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

        void Start()
        {
            this._rigidbody2D = GetComponent<Rigidbody2D>();
            this._animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            this.ValidateState();
        }

        private void ValidateState()
        {
            this._nextStateChange -= Time.deltaTime;

            if (this._nextStateChange > 0)
            {
                return;
            }

            this.RandomDirection();
            this.Move();
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
    }
}