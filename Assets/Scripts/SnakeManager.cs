using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var position = Utils.GetRandomPosition(ToolManager.Instance.range.x, 
            ToolManager.Instance.range.y,
            ToolManager.Instance.range.z);

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
        var position = Utils.GetRandomPosition(ToolManager.Instance.range.x, 
            ToolManager.Instance.range.y,
            ToolManager.Instance.range.z);
        
        StartCoroutine(LateReborn(snake, position));
    }

    private IEnumerator LateReborn(SnakeSettings setting, Vector3 position)
    {
        yield return new WaitForSeconds(waitTime);
        CreateSnake(setting, position);
    }
}