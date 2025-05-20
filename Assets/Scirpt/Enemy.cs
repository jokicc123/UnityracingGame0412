using UnityEngine;
using UnityEngine.AI;

namespace CHANG
{
    public class Enemy : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Transform playerTransform;
        public Animator animator;

        public float followDistance = 2f;   // 跟在玩家後方
        public float attackRange = 1.2f;    // 攻擊距離
        public float attackCooldown = 1.5f; // 攻擊冷卻時間
        private float lastAttackTime;
        public bool canChase = true; // 是否可以追擊玩家

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            var player = FindAnyObjectByType<Player>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
          

        }

        void Update()
        {
            if (!canChase) return; // 如果不能追擊，則不執行以下邏輯
            if (playerTransform == null) return;

            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= attackRange)
            {
                agent.SetDestination(transform.position); // 停止移動
                animator.SetFloat("移動", 0f);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    transform.LookAt(playerTransform); // 確保面向玩家
                    animator.SetTrigger("攻擊");
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                Vector3 behindPos = playerTransform.position - playerTransform.forward * followDistance;
                agent.SetDestination(behindPos);
                animator.SetFloat("移動", agent.velocity.magnitude);

                float speed = agent.velocity.magnitude;
                animator.SetFloat("移動", speed);

            }
       
        }
    }
}
