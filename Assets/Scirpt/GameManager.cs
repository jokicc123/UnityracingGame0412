using UnityEngine;
using System.Collections;
using TMPro;
，using Unity.VisualScripting;

namespace CHANG
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI countdownText;
        public int gameDuration = 360; // 遊戲時間（秒）
        public Player playerController; // 參考 Player 腳本
        private Enemy[] enemies; // 修正：新增 enemies 欄位
        public GameObject gameOverPanel; // 遊戲結束面板
        public GameObject winPanel; // 遊戲結束面板
        public GameObject EndPoint; // 終點物件
        public UIManager uiManager;



        void Start()
        {
            // 開始時隱藏遊戲結束面板
            if (gameOverPanel&& winPanel != null)
            {
                gameOverPanel.SetActive(false);
                winPanel.SetActive(false);
            }
            // 修正：使用 FindObjectsByType 並指定排序模式
            enemies = Object.FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            playerController.canMove = false; // 禁止移動
            foreach (var enemy in enemies)
            {
                enemy.canChase = false; // 禁止敵人追擊
            }
            StartCoroutine(GameFlow());
        }

        IEnumerator GameFlow()
        {
            // 預備倒數 3, 2, 1
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            // 顯示「開始逃跑」
            countdownText.text = "開始逃跑";
            yield return new WaitForSeconds(1f);
            playerController.canMove = true; // 恢復移動
            foreach (var enemy in enemies)
            {
                enemy.canChase = true; // 禁止敵人追擊
            }

            // 開始遊戲倒數計時
            yield return StartCoroutine(GameTimer());
        }

        IEnumerator GameTimer()
        {
            int timeLeft = gameDuration;

            while (timeLeft > 0)
            {
                int minutes = timeLeft / 60;
                int seconds = timeLeft % 60;
                countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                yield return new WaitForSeconds(1f);
                timeLeft--;
            }

            countdownText.text = "00:00"; // 顯示時間到
            TriggerGameOver("你被拋棄了！");
        }


        public void TriggerWin(Player player)
        {
            StopAllCoroutines(); // 停止其他計時器或流程
            player.canMove = false; // 玩家停止移動
            player.ani.SetFloat("移動", 0); // 停止動畫（你要確定 ani 是 public 或有存取權限）

            foreach (var enemy in enemies)
            {
                enemy.canChase = false; // 停止敵人追擊
            }

            winPanel.SetActive(true); // 顯示勝利畫面
            countdownText.text = "你逃跑成功了！";
            uiManager.ShowWin();
        }
        public void TriggerGameOver(string message)
        {
            StopAllCoroutines();

            countdownText.text = message;
            playerController.canMove = false;

            foreach (var enemy in enemies)
            {
                enemy.canChase = false;
            }
            uiManager.ShowGameOver();
            gameOverPanel.SetActive(true);
        }
       
        public void TriggerEnemyAttackGameOver(string message)
        {
            StopAllCoroutines();

            countdownText.text = message;
            playerController.canMove = false;

            foreach (var enemy in enemies)
            {
                enemy.canChase = false;
            }

            uiManager.ShowGameOver();
            gameOverPanel.SetActive(true);
        }




    }
}
