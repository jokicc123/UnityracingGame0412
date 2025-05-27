using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CHANG
{
    public class Enemy : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Transform playerTransform;
        private Animator animator;

        [Header("參數設定")]
        public float followDistance = 2f;     // 與玩家保持的距離
        public float attackRange = 2.5f;      // 攻擊距離
        public float attackCooldown = 1.5f;   // 攻擊冷卻秒數
        public bool canChase = true;          // 是否可追擊玩家
        public bool isAttacking = false;      // 是否正在攻擊

        private float lastAttackTime;
        private Coroutine attackCoroutine;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            var player = FindAnyObjectByType<Player>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("找不到玩家！");
            }
        }

        void Update()
        {
            if (!canChase || playerTransform == null) return;

            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= attackRange)
            {
                // 停止移動並面向玩家
                agent.SetDestination(transform.position);
                agent.velocity = Vector3.zero;
                animator.SetFloat("移動", 0f);
                transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

                if (Time.time - lastAttackTime >= attackCooldown && !isAttacking)
                {
                    isAttacking = true;
                    animator.SetTrigger("攻擊");
                    transform.LookAt(playerTransform);
                    lastAttackTime = Time.time;

                    if (attackCoroutine != null)
                        StopCoroutine(attackCoroutine);
                    attackCoroutine = StartCoroutine(PerformAttackAfterDelay(0.4f)); // 0.4 秒後觸發傷害
                }
            }
            else
            {
                Vector3 targetPos = playerTransform.position - playerTransform.forward * followDistance;
                agent.SetDestination(targetPos);
                animator.SetFloat("移動", agent.velocity.magnitude);
            }
        }

        private IEnumerator PerformAttackAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);


            isAttacking = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                animator.SetTrigger("攻擊");
                player.OnAttacked(); // 觸發攻擊事件
            }
        }

        public void OnPlayerDeath()
        {
            canChase = false;
            agent.SetDestination(transform.position);
            animator.SetFloat("移動", 0f);
        }
    }
}
