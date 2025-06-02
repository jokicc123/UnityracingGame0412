using UnityEngine;
using UnityEngine.SceneManagement;
namespace CHANG
{
    public class MainMenu : MonoBehaviour
    {
        public void LevelSelectManager()
        {
           
              SceneManager.LoadScene("選擇關卡");
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("主選單");
        }

        public void BackTutorialManager()
        {
            SceneManager.LoadScene("操作教學");
        }

        public void LevelContent()
        {
            SceneManager.LoadScene("遊戲說明");
        }


        public void OnClickLoadLevel()
        {
            SceneManager.LoadScene("遊戲場景");
        }

        public void QuitGame()
        {
            Application.Quit();

        }       

    }
}
