using UnityEngine;
using System.Collections;
using TMPro;

namespace CHANG
{
    public class CountdownTimer : MonoBehaviour
    {
        public TextMeshProUGUI countdownText;
        public int gameDuration = 120; // 遊戲時間（秒）
        public Player playerController; // 參考 Player 腳本
        private Enemy[] enemies; // 修正：新增 enemies 欄位

        void Start()
        {
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
                // 顯示格式：mm:ss
                int minutes = timeLeft / 60;
                int seconds = timeLeft % 60;
                countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                yield return new WaitForSeconds(1f);
                timeLeft--;
            }

            // 顯示「你被煮來吃了」
            countdownText.text = "你被煮來吃了";
            playerController.canMove = false; // 禁止移動

        }
    }
}
