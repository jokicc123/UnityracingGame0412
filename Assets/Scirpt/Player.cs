using UnityEngine;
using System.Collections;

namespace CHANG
{
    public class Player : MonoBehaviour
    {
        public float rotationSpeed = 10f;
        public float normalSpeed = 5f;
        public float boostedSpeed = 10f;
        public float slowedSpeed = 2f;
        public float effectDuration = 5f;
        public float jumpHeight = 2f;
        public float jumpSpeed = 4f;
        public Transform cameraTransform;
        private CharacterController chacon;
        public Animator ani { get; private set; }
        public float terrainSpeedMultiplier = 0.5f;
        private bool isOnTerrain = false;
        public bool canMove = true;
        public bool isDead = false;
        public GameObject chickenPrefab;
        public GameManager gameManager;
        private float currentSpeed;
        private Coroutine speedCoroutine;

        // 跳躍相關
        private bool isJumping = false;
        private float jumpProgress = 0f;
        private float lastYOffset = 0f;

        protected void Awake()
        {
            chacon = GetComponent<CharacterController>();
            ani = GetComponent<Animator>();

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            currentSpeed = normalSpeed;
        }

        void Update()
        {
            if (!canMove) return;

            Move();
            CheckTerrainBelow();
            HandleJump();
        }

        void Move()
        {
            if (!canMove) return;

            float forwardInput = Input.GetAxis("Vertical");  // W/S
            float turnInput = Input.GetAxis("Horizontal");   // A/D

            // 旋轉角色
            if (Mathf.Abs(turnInput) > 0.1f)
            {
                float turnAmount = turnInput * rotationSpeed * Time.deltaTime;
                transform.Rotate(0, turnAmount, 0);
            }

            // 前進方向
            Vector3 moveDir = transform.forward * forwardInput;

            if (moveDir.magnitude >= 0.1f)
            {
                float finalSpeed = currentSpeed;
                if (isOnTerrain)
                    finalSpeed *= terrainSpeedMultiplier;

                Vector3 horizontalMove = moveDir.normalized * finalSpeed * Time.deltaTime;

                // 水平移動（跳躍時仍可移動）
                chacon.Move(horizontalMove);
            }
            else
            {
                // 不移動時，必須傳 Vector3.zero 讓CharacterController處理碰撞
                chacon.Move(Vector3.zero);
            }

            ani.SetFloat("移動", Mathf.Abs(forwardInput));  // 移動動畫

            // 按空白鍵觸發跳躍（只在非跳躍狀態下）
            if (!isJumping && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpProgress = 0f;
                lastYOffset = 0f;
                ani.SetTrigger("跳躍");  // 若有跳躍動畫
            }
        }

        void HandleJump()
        {
            if (!isJumping) return;

            jumpProgress += Time.deltaTime * jumpSpeed;
            if (jumpProgress > 1f) jumpProgress = 1f;

            float currentYOffset = jumpHeight * Mathf.Sin(Mathf.PI * jumpProgress);
            float deltaYOffset = currentYOffset - lastYOffset;

            Vector3 verticalMove = new Vector3(0, deltaYOffset, 0);
            chacon.Move(verticalMove);

            lastYOffset = currentYOffset;

            if (jumpProgress >= 1f)
            {
                isJumping = false;
                lastYOffset = 0f;
            }
        }

        void CheckTerrainBelow()
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(ray, 2f);

            bool steppedOnRoad = false;
            bool steppedOnTerrain = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("道路")) steppedOnRoad = true;
                if (hit.collider.GetComponent<Terrain>() != null) steppedOnTerrain = true;
            }

            isOnTerrain = !steppedOnRoad && steppedOnTerrain;
        }

        public void OnAttacked()
        {
            if (isDead) return;
            isDead = true;

            if (chickenPrefab != null)
            {
                Instantiate(chickenPrefab, transform.position, Quaternion.identity);
            }

            gameObject.SetActive(false);

            if (gameManager != null)
            {
                gameManager.TriggerEnemyAttackGameOver("你被煮來吃了！");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("終點") && !isDead)
            {
                gameManager.TriggerWin(this);
            }

            // 吃道具（加速 / 減速）
            if (other.CompareTag("加速"))
            {
                ApplySpeedBoost();
                MusicManager.Instance.PlayConsumeitemClip();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("減速"))
            {
                ApplySpeedSlow();
                MusicManager.Instance.PlayConsumeitemClip();
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("敵人"))
            {
                MusicManager.Instance.PlayScream();
            }

        }

        public void ApplySpeedBoost()
        {
            if (speedCoroutine != null) StopCoroutine(speedCoroutine);
            speedCoroutine = StartCoroutine(TempSpeedChange(boostedSpeed));
        }

        public void ApplySpeedSlow()
        {
            if (speedCoroutine != null) StopCoroutine(speedCoroutine);
            speedCoroutine = StartCoroutine(TempSpeedChange(slowedSpeed));
        }

        IEnumerator TempSpeedChange(float newSpeed)
        {
            currentSpeed = newSpeed;
            yield return new WaitForSeconds(effectDuration);
            currentSpeed = normalSpeed;
        }
    }
}
