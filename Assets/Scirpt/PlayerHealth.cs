using UnityEngine;

namespace CHANG
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 3;
        private int currentHealth;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            Debug.Log("玩家受傷！當前血量：" + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("玩家死亡");
            // 可以加上動畫、關卡結束、UI 顯示等
            gameObject.SetActive(false); // 暫時隱藏玩家
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }
    }
}
