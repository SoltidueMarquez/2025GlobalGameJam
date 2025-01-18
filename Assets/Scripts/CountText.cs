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
    private string name = "";

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
        name = txt;
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

        if (minutes > 0)
        {
            countdownText.text = $"{(name == "" ? string.Empty : name + "：")}{minutes:D2}:{seconds:D2}"; // 格式化为 "mm:ss"
        }
        else if (seconds < 10) 
        {
            countdownText.text = $"{(name == "" ? string.Empty : name + "：")}{seconds:D1}";
        }
    }
}