using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [SerializeField] private float velocity;
    
        void Start()
        {
            this._rigidbody2D = GetComponent<Rigidbody2D>();
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

            if (horizontal != 0)
            {
                this.transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f);
            }
        }
    }
}
