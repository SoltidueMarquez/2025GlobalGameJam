using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [Header("游戏倒计时")] public CountText countTime;
    public float startTime = 90f;
    
    private UnityEvent onTimesUp = new UnityEvent();
    
    private void Start()
    {
        onTimesUp.AddListener(GameOver);
        Time.timeScale = 1;
        countTime.Init(startTime, onTimesUp);
    }

    private void GameOver()
    {
        Time.timeScale = 0;
    }
}
