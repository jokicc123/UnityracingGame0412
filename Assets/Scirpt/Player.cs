using UnityEngine;
using System;

namespace CHANG
{
    public class Player : MonoBehaviour
    {
        [Header("移動參數")]
        public float walkSpeed = 5f;
        public float runSpeed = 10f;
        public float rotationSpeed = 10f;

        [Header("跳躍參數")]
        public float jumpHeight = 2f;
        public float jumpSpeed = 4f;

        [Header("組件")]
        public Transform cameraTransform;
        private CharacterController controller;
        public Animator ani { get; private set; }

        // 玩家死亡委派事件
        public static Action OnPlayerDead;

        private bool isJumping = false;
        private float jumpProgress = 0f;
        private float lastYOffset = 0f;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            ani = GetComponent<Animator>();

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            Move();
            HandleJump();
        }

        void Move()
        {
            float forwardInput = Input.GetAxis("Vertical");   // W/S
            float turnInput = Input.GetAxis("Horizontal");    // A/D

            // 旋轉角色
            if (Mathf.Abs(turnInput) > 0.1f)
            {
                transform.Rotate(0, turnInput * rotationSpeed * Time.deltaTime, 0);
            }

            // 移動方向
            Vector3 moveDir = transform.forward * forwardInput;

            if (moveDir.magnitude >= 0.1f)
            {
                // 判斷是否跑步
                float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

                Vector3 horizontalMove = moveDir.normalized * speed * Time.deltaTime;
                controller.Move(horizontalMove);
            }
            else
            {
                controller.Move(Vector3.zero);
            }

            // 更新動畫
            if (ani != null)
                ani.SetFloat("移動", Mathf.Abs(forwardInput));

            // 按空白鍵跳躍
            if (!isJumping && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpProgress = 0f;
                lastYOffset = 0f;
                ani?.SetTrigger("跳躍");
            }
        }

        void HandleJump()
        {
            if (!isJumping) return;

            jumpProgress += Time.deltaTime * jumpSpeed;
            jumpProgress = Mathf.Clamp01(jumpProgress);

            float currentYOffset = jumpHeight * Mathf.Sin(Mathf.PI * jumpProgress);
            float deltaYOffset = currentYOffset - lastYOffset;

            controller.Move(new Vector3(0, deltaYOffset, 0));
            lastYOffset = currentYOffset;

            if (jumpProgress >= 1f)
            {
                isJumping = false;
                lastYOffset = 0f;
            }
        }

        // 玩家死亡
        public void Dead()
        {
            ani?.SetTrigger("死亡");
            OnPlayerDead?.Invoke();
        }

        // 被敵人抓到
        public void CaughtByEnemy()
        {
            Debug.Log("玩家被抓到！");
            Dead(); // 呼叫死亡事件
        }
    }
}
