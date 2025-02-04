﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    [Tooltip("身体间距")] public float interval = 1;
    
    public GameObject snakePrefab;
    public List<SnakeSet> settings = new List<SnakeSet>();

    [Header("泡泡相关")]
    [Tooltip("吸收半径")] public float radius = 2f;
    [Tooltip("爆出泡泡的概率"), Range(0, 1)] public float createBubbleRate;
    
    [Header("重生")]
    [Tooltip("x,z,检测半径")]public Vector3 range;
    [Tooltip("重生时间")] public float waitTime;

    private List<Snake> snakeList = new List<Snake>();
    private void Start()
    {
        var num = 0;
        foreach (var setting in settings)
        {
            CreateSnake(setting);
            setting.uiParent.gameObject.SetActive(true);
            num++;
            if (num >= SceneLoadManager.Instance.playerNum) break;
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
        snake.Init(moveSpeed, speedRange, steerSpeed, setting, interval, radius, createBubbleRate);
        
        snakeList.RemoveAll(item => item == null);
        snakeList.Add(snake);
    }
    
    public void Reborn(SnakeSet snake)
    {
        if (snake.ifReborn) return;//避免重复
        snake.ifReborn = true;//正在重生
        var position = Utils.GetRandomPosition(range.x, range.y, range.z);
        var onEnd = new UnityEvent();
        onEnd.AddListener(() => 
        {
            CreateSnake(snake, position);  // 创建蛇
            snake.ifReborn = false;
        });
        ToolManager.Instance.CreateMark(position, waitTime, onEnd, snake.settings.color);
    }

    public void AllSpeedUp(float deltaSpeed)//全体加速
    {
        moveSpeed += deltaSpeed;//增加基础速度
        snakeList.RemoveAll(item => item == null);
        foreach (var snake in snakeList)
        {
            snake.ChangeSpeed(deltaSpeed);
        }
    }

    public List<string> GetBigger(out int maxNum)
    {
        var res = new List<string>();
        var maxScore = settings.Select(set => set.score).Prepend(0).Max();
        foreach (var set in settings)
        {
            if (set.score == maxScore)
            {
                res.Add(set.name);
            }
        }
        maxNum = maxScore;
        return res;
    }
}

[Serializable]
public class SnakeSet
{
    public string name;
    public SnakeSettings settings;
    public Transform uiParent;
    public Text scoreText;
    public int score;
    [HideInInspector] public bool ifReborn = false;
}