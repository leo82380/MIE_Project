using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MIE.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private int levelNumber;
        private TextMeshProUGUI levelText;

        private void Awake()
        {
            levelText = GetComponentInChildren<TextMeshProUGUI>();
            levelText.text = levelNumber.ToString();
        }

        public void LoadLevel()
        {
            string levelName = $"Level {levelNumber}";
            SceneManager.LoadScene(levelName);
        }
    }
}