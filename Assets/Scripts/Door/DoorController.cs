using Helper;
using UnityEngine;

namespace Door
{
    public class DoorController : MonoBehaviour
    {

        private Animator _animator;
        
        void Start()
        {
            this._animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Open()
        {
            this._animator.SetTrigger(AnimationParameter.OpenDoor);
        }
        
        public void Close()
        {
            this._animator.SetTrigger(AnimationParameter.CloseDoor);
        }
    }
}
