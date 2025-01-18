using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [Header("蛇头设置")] 
    [SerializeField] private float moveSpeed;
    private float steerSpeed;
    private float absorbRadius;
    private Vector2 speedRange;
    private SnakeSettings snakeSettings;
    private Quaternion targetRotation;  // 目标旋转
    private KeyCode verKey, horKey;
    private Rigidbody rb;

    [Header("蛇身跟随设置")]
    private float lateActiveTime = 0.5f;//激活时间
    private float interval = 1;//身体间距
    private List<SnakeBody> bodyParts = new List<SnakeBody>(); // 身体部分列表
    private Transform lastBodyPart; // 最后一部分，便于定位新生成位置
    private List<Vector3> headPositions = new List<Vector3>(); // 蛇头位置列表
    private int positionListSize; // 控制位置列表的长度
    private int gap;
    private float createBubbleRate;

    public void Init(float moveSpeed, Vector2 speedRange, float steerSpeed, SnakeSettings snakeSettings,
        float lateActiveTime, float interval, float absorbRadius, float createBubbleRate)
    {
        this.moveSpeed = moveSpeed;
        this.speedRange = speedRange;
        this.steerSpeed = steerSpeed;
        this.snakeSettings = snakeSettings;
        this.lateActiveTime = lateActiveTime;
        this.interval = interval;
        this.absorbRadius = absorbRadius;
        this.createBubbleRate = createBubbleRate;
        
        verKey = KeyCode.Space;
        horKey = KeyCode.Space;
        
        rb = GetComponent<Rigidbody>();
        this.gameObject.tag = snakeSettings.playerTag;
        UpdateGap();
        lastBodyPart = transform;// 初始化头部位置
        headPositions.Add(transform.position); // 初始化位置列表，保存初始位置
    }
    

    #region 移动控制
    private void Update()
    {
        // 头部移动
        MoveHead();
        // 身体部分根据位置列表更新
        MoveBodyParts();
        //吸收泡泡
        AbsorbBubble();
    }
    
    /// <summary>
    /// 控制蛇头移动。
    /// </summary>
    private void MoveHead()
    {
        moveSpeed = Mathf.Clamp(moveSpeed, speedRange.x, speedRange.y);//限制速度
        Vector3 moveDirection = transform.forward * (moveSpeed * Time.deltaTime);
        rb.MovePosition(rb.position + moveDirection);

        // 根据按键设置方向
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKeyDown(snakeSettings.up)) verKey = snakeSettings.up;
        if (Input.GetKeyDown(snakeSettings.down)) verKey = snakeSettings.down;
        
        if (Input.GetKeyDown(snakeSettings.left)) horKey = snakeSettings.left;
        if (Input.GetKeyDown(snakeSettings.right)) horKey = snakeSettings.right;
        
        if (Input.GetKey(snakeSettings.up) && verKey == snakeSettings.up) inputDirection.z = 1f;
        if (Input.GetKey(snakeSettings.down) && verKey == snakeSettings.down) inputDirection.z = -1f;

        if (Input.GetKey(snakeSettings.left) && horKey == snakeSettings.left) inputDirection.x = -1f;
        if (Input.GetKey(snakeSettings.right) && horKey == snakeSettings.right) inputDirection.x = 1f;

        // 计算目标旋转
        if (inputDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.deltaTime);
        }

    }
    #endregion
    
    #region 蛇身跟随
    private void FixedUpdate()
    {
        UpdateHeadPositions();// 更新头部位置列表
        UpdateGap();
    }
    
    /// <summary>
    /// 更新蛇头位置列表。
    /// </summary>
    private void UpdateHeadPositions()
    {
        headPositions.Insert(0, transform.position);  // 插入蛇头当前位置到列表开头
        if (headPositions.Count > positionListSize)
        {
            for (int i = 1; i <= headPositions.Count - positionListSize + 1; i++)
                headPositions.RemoveAt(headPositions.Count - i); // 删除多余的位置
        }
    }
    
    /// <summary>
    /// 让身体部分根据位置列表更新。
    /// </summary>
    private void MoveBodyParts()
    {
        if (bodyParts.Count == 0) return;

        // 身体部分按顺序更新位置
        for (int i = 0; i < bodyParts.Count; i++)
        {
            var body = bodyParts[i];
            if (!body.ifActive) continue;

            // 获取当前位置列表中的相应位置
            Vector3 targetPosition = headPositions[Mathf.Min(i * gap, headPositions.Count - 1)];

            // 平滑跟随目标位置
            body.transform.position = Vector3.Lerp(body.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 朝向目标位置
            body.transform.LookAt(targetPosition);
        }
    }
    
    private void UpdateGap()
    {
        gap = Mathf.RoundToInt(interval / (Time.fixedDeltaTime * moveSpeed));
        positionListSize = gap * (bodyParts.Count + 1);
    }
    #endregion

    /// <summary>
    /// 添加新的身体部分。
    /// </summary>
    public void AddBodyPart()
    {
        // 生成新身体部分并设置其位置和旋转
        var newBodyPart = Instantiate(snakeSettings.bodyPrefab, lastBodyPart.position, lastBodyPart.rotation);
        // 更新最后一部分的引用
        lastBodyPart = newBodyPart.transform;
        lastBodyPart.SetParent(this.transform.parent);
        //初始化身体
        var newBody = newBodyPart.GetComponent<SnakeBody>();
        newBody.Init(lateActiveTime, snakeSettings.playerTag);
        bodyParts.Add(newBody);// 添加到身体列表
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Die()
    {
        SnakeManager.Instance.Reborn(this.snakeSettings);
        foreach (var body in bodyParts.Where(body => Random.Range(0f, 1f) < createBubbleRate))
        {
            ToolManager.Instance.CreateCandy(body.transform.position);
        }
        Destroy(gameObject.transform.parent.gameObject);
    }

    //蛇头碰撞
    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckIfPlayer(collision, snakeSettings.playerTag))
        {
            collision.gameObject.GetComponent<Snake>().Die();
        }
    }

    //改变速度
    public void ChangeSpeed(float delta)
    {
        moveSpeed += delta;
    }
    
    //吸收泡泡
    private void AbsorbBubble()
    {
        // 获取所有在指定半径内的物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, absorbRadius);

        foreach (var collider in colliders)
        {
            // 检查物体是否挂载了Bubble脚本
            var bubble = collider.GetComponent<CandyBubble>();
            if (bubble != null)
            {
                // 计算物体到脚本携带者的方向
                Vector3 direction = (transform.position - collider.transform.position).normalized;

                // 平滑移动物体向脚本携带者靠近
                collider.transform.position = Vector3.MoveTowards(collider.transform.position, transform.position, speedRange.y * Time.deltaTime);
            }
        }
    }
}



