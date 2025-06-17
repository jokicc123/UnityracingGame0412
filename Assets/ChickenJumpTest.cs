using UnityEngine;

public class ChickenJumpTest : MonoBehaviour
{
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ani.SetTrigger("跳躍");
        }
    }
}


