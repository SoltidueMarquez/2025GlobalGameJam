using System.Collections;
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
    [Tooltip("激活时间")] public float lateActiveTime = 0.1f;
    [Tooltip("身体间距")] public float interval = 1;
    
    public GameObject snakePrefab;
    public List<SnakeSettings> settings = new List<SnakeSettings>();

    [Tooltip("x,z,检测半径")]public Vector3 range;
    [Tooltip("重生时间")] public float waitTime;
    
    private void Start()
    {
        foreach (var setting in settings)
        {
            CreateSnake(setting);
        }
    }


    public void CreateSnake(SnakeSettings setting)
    {
        var position = Utils.GetRandomPosition(range.x, range.y, range.z);

        CreateSnake(setting, position);
    }

    public void CreateSnake(SnakeSettings setting, Vector3 position)
    {
        var tmp = Instantiate(snakePrefab, this.transform);
        tmp.transform.position = position;
        
        var snake = tmp.GetComponentInChildren<Snake>();
        snake.Init(moveSpeed, steerSpeed, setting, lateActiveTime, interval);
    }
    
    public void Reborn(SnakeSettings snake)
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