using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [Header("Snake Settings")]
    public float moveSpeed = 5f;
    public float steerSpeed = 180f;
    public float rotateTime = 0.2f;
    [Range(0.1f, 0.9f)] public float bodyFollowRate;
    public GameObject bodyPrefab;

    private List<Transform> bodyParts = new List<Transform>(); // 身体部分列表
    private Transform lastBodyPart; // 最后一部分，便于定位新生成位置

    private void Start()
    {
        // 初始化头部位置
        lastBodyPart = transform;
    }

    private void Update()
    {
        // 头部移动
        MoveHead();

        // 身体部分跟随
        MoveBodyParts();

        // 按下空格添加身体
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddBodyPart();
        }
    }

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


    /// <summary>
    /// 让身体部分平滑跟随。
    /// </summary>
    private void MoveBodyParts()
    {
        Vector3 previousPosition = transform.position;

        foreach (Transform bodyPart in bodyParts)
        {
            // 平滑跟随前一部分的位置
            Vector3 nextPosition = Vector3.Lerp(bodyPart.position, previousPosition, bodyFollowRate * moveSpeed * Time.deltaTime);
            bodyPart.position = nextPosition;

            // 朝向前一部分
            bodyPart.LookAt(previousPosition);

            // 更新参考位置
            previousPosition = bodyPart.position;
        }
    }

    /// <summary>
    /// 添加新的身体部分。
    /// </summary>
    private void AddBodyPart()
    {
        // 生成新身体部分并设置其位置和旋转
        Transform newBodyPart = Instantiate(bodyPrefab, lastBodyPart.position, lastBodyPart.rotation).transform;

        // 更新最后一部分的引用
        lastBodyPart = newBodyPart;

        // 添加到身体列表
        bodyParts.Add(newBodyPart);
    }
}