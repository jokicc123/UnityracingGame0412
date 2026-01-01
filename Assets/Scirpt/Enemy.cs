using UnityEngine;
using UnityEngine.AI;
namespace CHANG
{
    public class Enemy : EnemyBase
    {
        private NavMeshAgent agent;
        private Animator ani;
        public float chaseDistance= 10f;


        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            ani = GetComponent<Animator>();
        }

        protected override void ChasePlayer()
        {
            if (player == null) return;

            // 永遠追玩家，不限制距離
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;
            ani?.SetFloat("移動", speed);
            // 玩家接觸判定可以用玩家自己的委派事件，不需要限制距離
            var playerCtrl = player.GetComponent<Player>();
            if (playerCtrl != null && Vector3.Distance(transform.position, player.position) < 1.5f)
            {
                playerCtrl.CaughtByEnemy();
            }
        }
    }

}

