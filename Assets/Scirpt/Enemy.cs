using UnityEngine;
namespace CHANG 
{
    public class EnemyController : MonoBehaviour
    {
        public Transform player;
        public float speed = 2f;
        private Animator animator;
        private bool ischase = false;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.position);

                if (distance > 1.5f)
                {
                    // 追擊
                    Vector2 direction = (player.position - transform.position).normalized;
                    transform.position += (Vector3)direction * speed * Time.deltaTime;

                    animator.SetBool("追擊", true);
                    animator.SetBool("攻擊", false);
                    ischase = true;
                }
                else
                {
                    // 停下來攻擊
                    animator.SetBool("追擊", false);
                    animator.SetBool("攻擊", true);
                    ischase = false;
                }
            }
        }
    }
}

