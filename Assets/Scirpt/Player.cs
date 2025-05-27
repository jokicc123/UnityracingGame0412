using Unity.VisualScripting;
using UnityEngine;


namespace CHANG

{
    public class Player : MonoBehaviour
    {
        // 移動速度（正常走路）
        public float moveSpeed = 4f;
        // 移動速度（跑步）
        public float runSpeed = 7f;
        // 轉向時的旋轉速度
        public float rotationSpeed = 10f;
        // 用來參考攝影機方向的 Transform
        public Transform cameraTransform;
        private CharacterController chacon; // 控制角色移動的元件
      public Animator ani { get; private set; }      // 控制動畫的元件
        public float terrainSpeedMultiplier = 0.5f; // 踩到 Terrain 時的減速倍率
        private bool isOnTerrain = false;
        public bool canMove = true;
        public bool isDead = false; // 是否死亡的狀態
        public GameObject chickenPrefab; // 指派烤雞 prefab
        public GameManager gameManager;


        float turnSmoothVelocity; // 用於平滑轉向的中間變數

       protected void Awake()
        {
            // 取得角色身上的 CharacterController 元件
            chacon = GetComponent<CharacterController>();

            // 取得角色身上的 Animator 元件
            ani = GetComponent<Animator>();

            // 如果沒有手動指定相機，則自動抓場景的主攝影機
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            // 每幀都呼叫移動方法
            if (!canMove) return; //  在這阻擋移動邏輯
            Move();
            CheckTerrainBelow();

        }

        void Move()
        {
            // 取得玩家輸入（水平 H = A/D鍵，垂直 V = W/S鍵）
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // 根據輸入方向建立一個新的方向向量
            Vector3 direction = new Vector3(h, 0f, v).normalized;

            float currentSpeed = 0f; // 當前移動速度

            // 當有輸入方向時（例如按了 W/A/S/D）
            if (direction.magnitude >= 0.1f)
            {
                // 如果按的是W或A/D，才進行旋轉
                if (v > 0 || h != 0)  // 只有前進或左右移動時才旋轉
                {
                    // 計算目標角度（根據輸入方向與攝影機角度）
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

                    // 平滑轉向角色朝目標角度
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                // 計算最終的移動方向
                Vector3 moveDir = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;

                // 判斷是否按住 Shift（跑步）
                currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

                if (isOnTerrain)
                {
                    currentSpeed *= terrainSpeedMultiplier;
                }


                // 如果按下S鍵，則角色朝反方向移動
                if (v < 0)
                {
                    moveDir = -moveDir; // 反轉移動方向，實現後退
                }

                // 用 CharacterController 移動角色
                chacon.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            }
            else
            {
                // 没有输入时保持角色旋转角度不变
                transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

                // 让角色停止移动
                chacon.Move(Vector3.zero);
            }

            // 控制动画播放速度（移动动画）
            ani.SetFloat("移動", direction.magnitude * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));

            // 显示当前速度到 Console
            Debug.Log("目前速度：" + currentSpeed);
        }

        void CheckTerrainBelow()
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(ray, 2f);

            bool steppedOnRoad = false;
            bool steppedOnTerrain = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("道路"))
                {
                    steppedOnRoad = true;
                }

                if (hit.collider.GetComponent<Terrain>() != null)
                {
                    steppedOnTerrain = true;
                }
            }

            // 優先：如果有踩到路，就不減速
            if (steppedOnRoad)
            {
                isOnTerrain = false;
            }
            else
            {
                isOnTerrain = steppedOnTerrain;
            }
        }
        public void OnAttacked()
        {
            if (isDead) return;

            isDead = true;

            if (chickenPrefab != null)
            {
                Instantiate(chickenPrefab, transform.position, Quaternion.identity);
            }

            // 隱藏玩家角色
            gameObject.SetActive(false);

            // 呼叫 GameManager 顯示失敗畫面
            if (gameManager != null)
            {
                gameManager.TriggerEnemyAttackGameOver("你被煮來吃了！");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("終點") && !isDead)
            {
                Debug.Log("抵達終點！");

                // 呼叫 GameManager 的勝利函式，傳入自己這個 Player 物件 (this)
                gameManager.TriggerWin(this);
            }
          
        }

    }
}
