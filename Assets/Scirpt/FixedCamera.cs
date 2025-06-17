using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 10f, -10f);

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position + offset;
        transform.rotation = Quaternion.Euler(45f, 0f, 0f); // 俯視角度
    }
}
