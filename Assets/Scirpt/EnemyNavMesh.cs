using System;
using UnityEngine;
using UnityEngine.AI;

namespace CHANG
{
    public class EnemyNavMesh : MonoBehaviour
    {
        public Transform player;      // 玩家
        public Transform endPoint;    // 可選的終點（例如逃跑點）
        public float speed = 3.5f;    // 移動速度
        public float attackDistance = 1.5f; // 攻擊距離
        private NavMeshAgent agent;   // 控制移動
        private Animator ani;

        private bool isAttacking = false;

        void Awake()
        {
            ani = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > attackDistance)
            {
                // 玩家距離遠 → 持續追擊
                if (isAttacking)
                {
                    ani.SetBool("攻擊", false);
                    ani.SetBool("移動", true);
                    isAttacking = false;
                }

                agent.isStopped = false; // 允許移動
                agent.SetDestination(player.position); // 持續追玩家
            }
            else
            {
                // 玩家在攻擊距離內 → 停下並攻擊
                if (!isAttacking)
                {
                    ani.SetBool("移動", false);
                    ani.SetBool("攻擊", true);
                    isAttacking = true;
                }

                agent.isStopped = true; // 停止移動
                LookAtPlayer(); // 面向玩家
            }

            CheckArrival(); // 可選檢查終點
        }

        private void LookAtPlayer()
        {
            // 使用Quaternion進行平滑旋轉，避免直接使用LookAt可能會造成問題
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f); // 5f可調整旋轉速度
        }

        private void CheckArrival()
        {
            if (Vector3.Distance(transform.position, endPoint.position) < 1f)
            {
                Debug.Log("Arrived at endpoint!");
            }
        }
    }
}
