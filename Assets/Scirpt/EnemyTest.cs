using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // 強制移動到一個 NavMesh 上的點
        agent.SetDestination(new Vector3(5, 0, 5));
    }
}
