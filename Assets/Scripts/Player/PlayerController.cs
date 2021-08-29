using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [SerializeField] private float velocity;

        private Animator _animator;
    
        void Start()
        {
            this._rigidbody2D = GetComponent<Rigidbody2D>();
            this._animator = GetComponentInChildren<Animator>();
        }
    
        void Update()
        {
            this.Movimentar();
        }

        private void Movimentar()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var newVelocity = new Vector2(horizontal, this._rigidbody2D.velocity.y).normalized * this.velocity;
            
            this._rigidbody2D.velocity = newVelocity;
            
            this._animator.SetBool(PlayerState.Moving, (horizontal != 0));

            if (horizontal != 0)
            {
                this.transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f);
            }
        }
    }
}
