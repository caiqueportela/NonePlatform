using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        
        private static int _playerLife = 3;
        
        void Start()
        {
        
        }
        
        void Update()
        {
            
        }

        public void ResetGame()
        {
            GameManager._playerLife = 3;
            SceneManager.LoadScene("Sala1");
        }
        
        public void TakeDamage(int damage)
        {
            GameManager._playerLife -= damage;
        }

        public int GetPlayerLife()
        {
            return GameManager._playerLife;
        }

        public void ChangeScene(SceneAsset scene)
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}
