using UnityEngine;
using System.Linq;
namespace CHANG
{
    using System.Linq;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public NPC[] allNPCs;

        private int rescuedCount = 0;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            Player.OnPlayerDead += HandlePlayerCaught;
        }

        void OnDestroy()
        {
            Player.OnPlayerDead -= HandlePlayerCaught;
        }

        public void RescueNPC()
        {
            rescuedCount++;
            PrintRescueStatus();

            if (rescuedCount >= allNPCs.Length)
            {
                Debug.Log("玩家勝利！");
            }
        }

        void PrintRescueStatus()
        {
            var rescuedNames = allNPCs.Where(n => n.rescued).Select(n => n.name);
            Debug.Log("已救援 NPC: " + string.Join(", ", rescuedNames));
        }

        void HandlePlayerCaught()
        {
            Debug.Log("遊戲失敗！");
        }
    }

}
