using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    [Header("游戏倒计时")] public CountText countTime;
    public float startTime = 90f;

    public GameObject countTextPrefab;
    
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

    public void CreateCountTimer(Transform parent, string title, float time, UnityEvent onEnd)
    {
        var timer = Instantiate(countTextPrefab, parent).GetComponent<CountText>();
        timer.Init(title, time, onEnd);
    }
}
