using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    CharacterController chacon;
    bool isJumping = false;
    float jumpHeight = 10f;
    float jumpSpeed = 4f;
    float jumpProgress = 0f;

    void Awake()
    {
        chacon = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpProgress = 0f;
        }

        if (isJumping)
        {
            jumpProgress += Time.deltaTime * jumpSpeed;

            float yOffset = jumpHeight * Mathf.Sin(Mathf.PI * jumpProgress);
            Vector3 move = new Vector3(0, yOffset, 0);

            chacon.Move(move * Time.deltaTime);

            if (jumpProgress >= 1f)
            {
                isJumping = false;
            }
        }
    }
}
