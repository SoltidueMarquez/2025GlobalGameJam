using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [Header("Snake Settings")]
    public float moveSpeed = 5f;
    public float steerSpeed = 180f;
    public float rotateTime = 0.2f;
    public GameObject bodyPrefab;
    public float lateActiveTime = 0.5f;
    private List<SnakeBody> bodyParts = new List<SnakeBody>(); // 身体部分列表
    private Transform lastBodyPart; // 最后一部分，便于定位新生成位置
    
    private List<Vector3> headPositions = new List<Vector3>(); // 蛇头位置列表
    private int positionListSize; // 控制位置列表的长度
    public int gap;
    
    private void Start()
    {
        lastBodyPart = transform;// 初始化头部位置
        headPositions.Add(transform.position); // 初始化位置列表，保存初始位置
        positionListSize = 2*gap;
    }

    private void Update()
    {
        // 头部移动
        MoveHead();

        // 身体部分根据位置列表更新
        MoveBodyParts();

        // 按下空格添加身体
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddBodyPart();
        }
    }

    private void FixedUpdate()
    {
        // 更新头部位置列表
        UpdateHeadPositions();
    }

    #region 移动控制
    /// <summary>
    /// 控制蛇头移动。
    /// </summary>
    private void MoveHead()
    {
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);

        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * (steerDirection * steerSpeed * Time.deltaTime));

        // 获取当前的输入
        if (Input.GetKey(KeyCode.W))
        {
            StartCoroutine(RotateHeadTo(new Vector3(0, 0, 0), rotateTime));  // 在1秒内旋转到(0, 0, 0)
        }
        else if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(RotateHeadTo(new Vector3(0, 180, 0), rotateTime));  // 在1秒内旋转到(0, 180, 0)
        }
        else if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(RotateHeadTo(new Vector3(0, -90, 0), rotateTime));  // 在1秒内旋转到(0, -90, 0)
        }
        else if (Input.GetKey(KeyCode.D))
        {
            StartCoroutine(RotateHeadTo(new Vector3(0, 90, 0), rotateTime));  // 在1秒内旋转到(0, 90, 0)
        }
    }
    
    /// <summary>
    /// 平滑旋转蛇头到目标角度。
    /// </summary>
    /// <param name="targetAngle">目标角度</param>
    /// <param name="duration">旋转持续的时间</param>
    private IEnumerator RotateHeadTo(Vector3 targetAngle, float duration)
    {
        Quaternion startRotation = transform.rotation;  // 当前角度
        Quaternion endRotation = Quaternion.Euler(targetAngle);  // 目标角度
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / duration);  // 平滑过渡
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;  // 确保最终角度精确到目标
    }
    #endregion


    /// <summary>
    /// 更新蛇头位置列表。
    /// </summary>
    private void UpdateHeadPositions()
    {
        headPositions.Insert(0, transform.position);  // 插入蛇头当前位置到列表开头
        if (headPositions.Count > positionListSize) 
        {
            headPositions.RemoveAt(headPositions.Count - 1);  // 删除多余的位置
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
            body.bodyTransform.position = Vector3.Lerp(body.bodyTransform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 朝向目标位置
            body.bodyTransform.LookAt(targetPosition);
        }
    }

    /// <summary>
    /// 添加新的身体部分。
    /// </summary>
    private void AddBodyPart()
    {
        positionListSize += gap;//位置列表增长
        // 生成新身体部分并设置其位置和旋转
        Transform newBodyPart = Instantiate(bodyPrefab, lastBodyPart.position, lastBodyPart.rotation).transform;

        // 更新最后一部分的引用
        lastBodyPart = newBodyPart;

        var newBody = new SnakeBody(newBodyPart, false);
        
        // 添加到身体列表
        bodyParts.Add(newBody);
        StartCoroutine(ActivateBody(newBody)); // 延时激活
    }

    private IEnumerator ActivateBody(SnakeBody body)
    {
        yield return new WaitForSeconds(lateActiveTime);
        body.ifActive = true;
    }
}


public class SnakeBody
{
    public Transform bodyTransform;
    public bool ifActive;

    public SnakeBody(Transform bodyTransform, bool ifActive)
    {
        this.bodyTransform = bodyTransform;
        this.ifActive = ifActive;
    }
}