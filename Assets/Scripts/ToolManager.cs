using System;
using System.Collections.Generic;
using StartScene;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public EatableTools candy;
    public EatableTools mystery;
    public EatableTools multiple;
    public GameObject markPrefab;
    
    [HideInInspector] public int curGenerateIndex;

    private void Start()
    {
        curGenerateIndex = 0;
    }

    private void Update()
    {
        candy.timer += Time.deltaTime;
        mystery.timer += Time.deltaTime;
        multiple.timer += Time.deltaTime;

        if (candy.timer >= candy.setting.infos[curGenerateIndex].intervalTime)
        {
            CreateCandy();
            candy.timer = 0f;  // 重置计时器
        }

        if (mystery.timer >= mystery.setting.infos[curGenerateIndex].intervalTime)
        {
            CreateMystery();
            mystery.timer = 0f;  // 重置计时器
        }
        
        if (multiple.timer >= multiple.setting.infos[curGenerateIndex].intervalTime)
        {
            CreateMultiple();
            multiple.timer = 0f;  // 重置计时器
        }
    }

    #region 工具封装
    private void CreateEatableTool<T>(EatableTools tool) where T : IInit
    {
        tool.list.RemoveAll(item => item == null);
        for (int i = 0; i < tool.setting.infos[curGenerateIndex].spawnCount; i++)
        {
            if (tool.list.Count >= tool.setting.maxNum)
            {
                return;
            }
            var tmp = Instantiate(tool.prefab, this.transform);
            tmp.GetComponentInChildren<T>().Init();
            tmp.transform.position = Utils.GetRandomPosition(tool.setting.range.x, tool.setting.range.y, tool.setting.range.z);
            tool.list.Add(tmp);
        }
    }

    private void CreateEatableTool<T>(EatableTools tool, Vector3 pos) where T : IInit
    {
        var tmp = Instantiate(tool.prefab, this.transform);
        tmp.GetComponentInChildren<T>().Init();
        tmp.transform.position = pos;
        tool.list.Add(tmp);
    }
    
    #endregion

    #region 创建增长气泡
    private void CreateCandy()
    {
        CreateEatableTool<CandyBubble>(candy);
    }
    public void CreateCandy(Vector3 pos)
    {
        CreateEatableTool<CandyBubble>(candy, pos);
    }
    #endregion

    #region 神秘气泡
    private void CreateMystery()
    {
        CreateEatableTool<MysteryBubble>(mystery);
    }
    public void CreateMystery(Vector3 pos)
    {
        CreateEatableTool<MysteryBubble>(mystery, pos);
    }
    #endregion

    #region 多重气泡
    private void CreateMultiple()
    {
        CreateEatableTool<MultipleBubble>(multiple);
    }
    public void CreateMultiple(Vector3 pos)
    {
        CreateEatableTool<MultipleBubble>(multiple, pos);
    }
    #endregion
    
    #region 复活标记
    public void CreateMark(Vector3 pos, float time, UnityEvent onEnd, Color color)
    {
        var mark = Instantiate(markPrefab, this.transform);
        mark.transform.position = pos + new Vector3(0, 2, 0);
        mark.GetComponent<Mark>().Init(time, onEnd, color);
    }
    #endregion

    public void UpdateGenerateInfoIndex()
    {
        curGenerateIndex = Mathf.Clamp(curGenerateIndex + 1, 0, 3);
    }
}

[Serializable]
public class EatableTools
{
    public GameObject prefab;
    public SpawnSettings setting;
    public List<GameObject> list = new List<GameObject>();
    [HideInInspector] public float timer = 0f;
}

public interface IInit
{
    public virtual void Init() { }
}