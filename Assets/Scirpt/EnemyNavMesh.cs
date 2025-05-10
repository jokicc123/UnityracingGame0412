using System;
using UnityEngine;
using UnityEngine.AI; // 引入 NavMesh 的命名空間

namespace CHANG
{
    public class EnemyNavMesh : MonoBehaviour
    {
        public Transform player;  // 玩家物件
        public Transform endPoint;  // 追擊結束的終點（可設置為某個位置）
        private NavMeshAgent agent;  // NavMeshAgent 用於控制移動
        public float speed = 3.5f;  // 速度

        void Start()
        {
            // 獲取 NavMeshAgent 元件
            agent = GetComponent<NavMeshAgent>();

            // 設定預設速度（可以根據需要調整）
            agent.speed = speed;
        }

        void Update()
        {
            // 每幀追蹤玩家
            ChasePlayer();

            // 檢查是否到達終點或碰到玩家
            CheckArrival();
        }

        // 始終追擊玩家
        private void ChasePlayer()
        {
            // 設定 NavMeshAgent 的目標為玩家
            agent.SetDestination(player.position);
        }

        // 檢查是否到達終點或碰到玩家
        private void CheckArrival()
        {
            // 檢查敵人是否到達終點
            if (Vector3.Distance(transform.position, endPoint.position) < 1f)
            {
                StopChasing();  // 到達終點停止追擊
                Debug.Log("Arrived at endpoint!");
            }

            // 檢查敵人是否碰到玩家
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                StopChasing();  // 撞到玩家停止追擊
                Debug.Log("Player is reached!");
            }
        }

        // 停止追擊玩家
        private void StopChasing()
        {
            // 停止移動
            agent.SetDestination(transform.position);
        }
    }
}
