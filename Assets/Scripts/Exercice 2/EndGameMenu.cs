using TMPro;
using UnityEngine;

namespace Exercice_2
{
    public class EndGameMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI endText;

        public void Start()
        {
            gameObject.SetActive(false);
            GameManager.Instance.OnGameWon += () => GameEnd(true);
            GameManager.Instance.OnGameLost += () => GameEnd(false);
            GameManager.Instance.OnGameStart += () => gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the endgame menu indicating if the player won or lost
        /// </summary>
        /// <param name="win"></param>
        public void GameEnd(bool win)
        {
            gameObject.SetActive(true);
            endText.text = "You've " + (win ? "won" : "lost") + "!";
        }
    }
}