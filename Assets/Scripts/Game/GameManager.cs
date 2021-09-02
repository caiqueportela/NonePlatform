using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        
        void Start()
        {
        
        }
        
        void Update()
        {
        
        }

        public void ChangeScene(SceneAsset scene)
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}
