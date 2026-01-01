using UnityEngine;
namespace CHANG
{
    public abstract class EnemyBase : MonoBehaviour
    {
        public Transform player;

        protected virtual void Update()
        {
            if (player != null)
                ChasePlayer();
        }

        protected abstract void ChasePlayer();
    }

}
