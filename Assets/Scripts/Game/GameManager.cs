using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        
        private static int _playerLife = 3;

        [SerializeField] private Image[] hearths;
        
        void Start()
        {
            this.UpdateHearths();
        }
        
        void Update()
        {
            
        }

        private void UpdateHearths()
        {
            for (int i = 0; i < this.hearths.Length; i++)
            {
                this.hearths[i].enabled = GameManager._playerLife >= (i + 1);
            }
        }

        public void ResetGame()
        {
            GameManager._playerLife = 3;
            SceneManager.LoadScene("Sala1");
        }
        
        public void TakeDamage(int damage)
        {
            GameManager._playerLife -= damage;
            this.UpdateHearths();
        }

        public int GetPlayerLife()
        {
            return GameManager._playerLife;
        }

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
