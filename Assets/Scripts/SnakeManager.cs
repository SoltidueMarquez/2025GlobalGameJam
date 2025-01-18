using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeManager : MonoBehaviour
{
    public static SnakeManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    public float moveSpeed = 5f;
    public float steerSpeed = 180f;
    public Vector2 speedRange = new Vector2(3, 20);
    [Tooltip("激活时间")] public float lateActiveTime = 0.1f;
    [Tooltip("身体间距")] public float interval = 1;
    
    public GameObject snakePrefab;
    public List<SnakeSet> settings = new List<SnakeSet>();

    [Header("泡泡相关")]
    [Tooltip("吸收半径")] public float radius = 10f;
    [Tooltip("爆出泡泡的概率"), Range(0, 1)] public float createBubbleRate;
    
    [Header("重生")]
    [Tooltip("x,z,检测半径")]public Vector3 range;
    [Tooltip("重生时间")] public float waitTime;
    
    private void Start()
    {
        foreach (var setting in settings)
        {
            CreateSnake(setting);
        }
    }


    public void CreateSnake(SnakeSet setting)
    {
        var position = Utils.GetRandomPosition(range.x, range.y, range.z);

        CreateSnake(setting, position);
    }

    public void CreateSnake(SnakeSet setting, Vector3 position)
    {
        var tmp = Instantiate(snakePrefab, this.transform);
        tmp.transform.position = position;
        
        var snake = tmp.GetComponentInChildren<Snake>();
        snake.Init(moveSpeed, speedRange, steerSpeed, setting, lateActiveTime, interval, radius, createBubbleRate);
    }
    
    public void Reborn(SnakeSet snake)
    {
        var position = Utils.GetRandomPosition(range.x, range.y, range.z);
        var onEnd = new UnityEvent();
        onEnd.AddListener(() => 
        {
            CreateSnake(snake, position);  // 创建蛇
        });
        ToolManager.Instance.CreateMark(position, waitTime, onEnd);
    }
}

[Serializable]
public struct SnakeSet
{
    public SnakeSettings settings;
    public Transform uiParent;
}