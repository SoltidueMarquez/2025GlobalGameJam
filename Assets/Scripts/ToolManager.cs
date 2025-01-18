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

    public EatableTools candy;
    public EatableTools mystery;

    public GameObject markPrefab;
    
    
    private void Start()
    {
        InvokeRepeating(nameof(CreateCandy), 0, candy.setting.intervalTime);
        InvokeRepeating(nameof(CreateMystery), 0, mystery.setting.intervalTime);
    }

    #region 工具封装
    private void CreateEatableTool<T>(EatableTools tool) where T : IInit
    {
        tool.list.RemoveAll(item => item == null);
        for (int i = 0; i < tool.setting.spawnCount; i++)
        {
            if (tool.list.Count >= tool.setting.maxNum)
            {
                return;
            }
            var tmp = Instantiate(tool.prefab, this.transform);
            tmp.GetComponent<T>().Init();
            tmp.transform.position = Utils.GetRandomPosition(tool.setting.range.x, tool.setting.range.y, tool.setting.range.z);
            tool.list.Add(tmp);
        }
    }

    private void CreateEatableTool<T>(EatableTools tool, Vector3 pos) where T : IInit
    {
        var tmp = Instantiate(tool.prefab, this.transform);
        tmp.GetComponent<T>().Init();
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

    #region 复活标记
    public void CreateMark(Vector3 pos, float time, UnityEvent onEnd)
    {
        var mark = Instantiate(markPrefab, this.transform);
        mark.transform.position = pos + new Vector3(0, 2, 0);
        mark.GetComponent<Mark>().Init(time, onEnd);
    }
    #endregion
}

[Serializable]
public class EatableTools
{
    public GameObject prefab;
    public SpawnSettings setting;
    public List<GameObject> list = new List<GameObject>();
}

public interface IInit
{
    public virtual void Init() { }
}