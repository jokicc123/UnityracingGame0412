using UnityEngine;

namespace CHANG
{
    public class UIManager : MonoBehaviour
    {
        public CanvasGroup winPanel;
        public CanvasGroup gameOverPanel;

        private void Start()
        {
            HideAll();
        }

        public void ShowWin()
        {
            HideAll();
            ShowPanel(winPanel);
        }

        public void ShowGameOver()
        {
            HideAll();
            ShowPanel(gameOverPanel);
        }

        public void HideAll()
        {
            HidePanel(winPanel);
            HidePanel(gameOverPanel);
        }

        private void ShowPanel(CanvasGroup cg)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        private void HidePanel(CanvasGroup cg)
        {
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }
}
