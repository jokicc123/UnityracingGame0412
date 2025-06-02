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
        float turnSmoothVelocity;

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

            // 可選：使用 Translate 額外位移
            float move = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * move);

            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //  Debug.Log("手動播放吃道具音效");
            //MusicManager.Instance.PlayConsumeitemClip();
            //}

        }

        void Move()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(h, 0f, v).normalized;

            if (direction.magnitude >= 0.1f)
            {
                if (v > 0 || h != 0)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                Vector3 moveDir = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;

                if (v < 0)
                {
                    moveDir = -moveDir;
                }

                float finalSpeed = currentSpeed;

                if (isOnTerrain)
                    finalSpeed *= terrainSpeedMultiplier;

                chacon.Move(moveDir.normalized * finalSpeed * Time.deltaTime);
            }
            else
            {
                chacon.Move(Vector3.zero);
            }

            // ✅ 只依照移動方向控制動畫，不乘 Shift 倍率
            ani.SetFloat("移動", direction.magnitude);
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
                // 通知 MusicManager 播放音效
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
