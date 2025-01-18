using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    public GameObject candyPrefab;
    public SpawnSettings candySetting;
    private List<GameObject> candyList = new List<GameObject>();

    public GameObject markPrefab;
    
    private void Start()
    {
        InvokeRepeating(nameof(CreateCandy), 0, candySetting.intervalTime);
    }
    
    private void CreateCandy()
    {
        candyList.RemoveAll(item => item == null);
        for (int i = 0; i < candySetting.spawnCount; i++)
        {
            if (candyList.Count >= candySetting.maxNum)
            {
                return;
            }
            var candy = Instantiate(candyPrefab, this.transform);
            candy.transform.position = Utils.GetRandomPosition(candySetting.range.x, candySetting.range.y, candySetting.range.z);
            candyList.Add(candy);
        }
    }

    public void CreateMark(Vector3 pos, float time, UnityEvent onEnd)
    {
        var mark = Instantiate(markPrefab, this.transform);
        mark.transform.position = pos;
        mark.GetComponent<Mark>().Init(time, onEnd);
    }
}