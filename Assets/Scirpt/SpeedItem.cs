using UnityEngine;

namespace CHANG
{
    public class SpeedItem : MonoBehaviour
    {
        public enum ItemType { SpeedUp, SpeedDown }
        public ItemType currentType = ItemType.SpeedUp;
        private Renderer rend;

        void Awake()
        {
            
            rend = GetComponent<Renderer>();
            UpdateColor();
        }
        void UpdateColor()
        {
            if (rend == null) return;

            switch (currentType)
            {
                case ItemType.SpeedUp:
                    rend.material.color = Color.green; // 綠色 = 加速
                    break;
                case ItemType.SpeedDown:
                    rend.material.color = Color.red;   // 紅色 = 減速
                    break;
            }
        }
        void ChangeType()
        {
            currentType = ItemType.SpeedDown;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                if (player != null)
                {
                    if (currentType == ItemType.SpeedUp)
                        player.ApplySpeedBoost();
                    else if (currentType == ItemType.SpeedDown)
                        player.ApplySpeedSlow();

                    Destroy(gameObject); // 撞到就消失
                }
            }
        }
    }
}
