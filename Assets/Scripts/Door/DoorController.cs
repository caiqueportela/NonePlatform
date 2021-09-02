using System.Collections;
using Game;
using Helper;
using UnityEditor;
using UnityEngine;

namespace Door
{
    public class DoorController : MonoBehaviour
    {
        private Animator _animator;

        [SerializeField] private SceneAsset destinyScene;

        private GameManager _gameManager;

        void Start()
        {
            this._animator = GetComponentInChildren<Animator>();
            this._gameManager = FindObjectOfType<GameManager>();
        }

        void Update()
        {
        }

        public IEnumerator GoToDestiny()
        {
            yield return new WaitForSeconds(3f);

            this._gameManager.ChangeScene(this.destinyScene);
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