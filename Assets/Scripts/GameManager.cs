using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    [Header("游戏倒计时")] public CountText countTime;
    public float startTime = 90f;
    public float deltaSpeed = 5f;
    public List<float> rushCheckTimeList = new List<float>();
    public GameObject countTextPrefab;

    public GameObject UITip;
    private UnityEvent onTimesUp = new UnityEvent();
    
    private void Start()
    {
        onTimesUp.AddListener(GameOver);
        Time.timeScale = 1;
        countTime.Init(startTime, onTimesUp);
        //设置倒计时加速
        foreach (var checkTime in rushCheckTimeList)
        {
            countTime.AddCheckPoint(checkTime, Rush);
        }
        UITip.SetActive(false);
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

    private void Rush()
    {
        SnakeManager.Instance.AllSpeedUp(deltaSpeed);
        ToolManager.Instance.UpdateGenerateInfoIndex();
        StartCoroutine(ShowUI());
    }

    private IEnumerator ShowUI()
    {
        UITip.SetActive(true);
        yield return new WaitForSeconds(1);
        UITip.SetActive(false);
    }
}
