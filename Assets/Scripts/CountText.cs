using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountText : MonoBehaviour
{
    private Text countdownText; // 用于显示倒计时的Text
    private float startTime;
    private float currentTime;
    private UnityEvent onEnd;
    private string title = "";
    private List<CheckPoint> checkpoints = new List<CheckPoint>();

    public void Init(float time, UnityEvent onEnd)
    {
        this.startTime = time;
        this.onEnd = onEnd;
        countdownText = this.transform.GetComponent<Text>();
        currentTime = startTime;
        UpdateCountdownText();
    }

    public void Init(string txt, float time, UnityEvent onEnd)
    {
        title = txt;
        Init(time, onEnd);
    }
    
    void Update()
    {
        // 更新倒计时
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 每帧减少时间
            UpdateCountdownText();
        }
        else
        {
            countdownText.text = "Time's Up!";
            onEnd.Invoke();
            Destroy(gameObject);//销毁自己
        }
    }
    
    // 更新Text显示为 "分:秒" 格式
    private void UpdateCountdownText()
    {
        var minutes = Mathf.FloorToInt(currentTime / 60); // 获取分钟部分
        var seconds = Mathf.FloorToInt(currentTime % 60); // 获取秒数部分

        if (minutes <= 0 && seconds < 10) 
        {
            countdownText.text = $"{(title == "" ? string.Empty : title + "：")}{seconds:D1}";
        }
        else
        {
            countdownText.text = $"{(title == "" ? string.Empty : title + "：")}{minutes:D2}:{seconds:D2}"; // 格式化为 "mm:ss"
        }

        if (checkpoints.Count > 0)
        {
            Check();
        }
    }

    private void Check()
    {
        foreach (var point in checkpoints)
        {
            if (currentTime <= point.checkTime)
            {
                point.action.Invoke();
                point.ifDone = true;
            }
        }
        checkpoints.RemoveAll(item => item.ifDone);
    }

    public void AddCheckPoint(float checkTime, UnityAction action)
    {
        checkpoints.Add(new CheckPoint(checkTime, action));
    }
}

public class CheckPoint
{
    public float checkTime;
    public UnityAction action;
    public bool ifDone;

    public CheckPoint(float checkTime, UnityAction action)
    {
        this.checkTime = checkTime;
        this.action = action;
        ifDone = false;
    }
}