using UnityEngine;
namespace CHANG
{
    public class SpeedItem : MonoBehaviour
    {
        public enum ItemType { SpeedUp, SpeedDown }
        public ItemType currentType = ItemType.SpeedUp;

        public float changeDelay = 10f; // 幾秒後變身

        private Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer>();
            UpdateColor(); // 根據狀態變顏色
            Invoke("ChangeType", changeDelay);
        }

        void ChangeType()
        {
            currentType = ItemType.SpeedDown;
            UpdateColor(); // 更新顏色表示已變身
        }

        void UpdateColor()
        {
            if (rend == null) return;

            switch (currentType)
            {
                case ItemType.SpeedUp:
                    rend.material.color = Color.green;
                    break;
                case ItemType.SpeedDown:
                    rend.material.color = Color.red;
                    break;
            }
        }
    }
}


