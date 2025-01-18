using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public GameObject countTextPrefab;

    [Header("冲刺")]
    public List<float> rushCheckTimeList = new List<float>();
    public GameObject UITip;
    private UnityEvent onTimesUp = new UnityEvent();
    
    [Header("游戏结束页面")] 
    public GameObject gameOverPanel;
    public Text winText;
    public Button backButton;
    public Button restartButton;

    public PauseUI pauseUI;
    
    private void Start()
    {
        onTimesUp.AddListener(GameOver);
        countTime.Init(startTime, onTimesUp);
        //设置倒计时加速
        foreach (var checkTime in rushCheckTimeList)
        {
            countTime.AddCheckPoint(checkTime, Rush);
        }
        UITip.SetActive(false);
        if (AudioManager.Instance != null)
        {
            if (SceneLoadManager.Instance.playerNum <= 2)
            {
                AudioManager.Instance.PlayMusic("Game1");
            }
            else
            {
                AudioManager.Instance.PlayMusic("Game2");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUI.Pause();
        }
    }

    #region 游戏结束
    private void GameOver()
    {
        Time.timeScale = 0;
        InitGameOverPanel();
    }

    private void InitGameOverPanel()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayRandomSound("GameOver");
        }
        backButton.onClick.AddListener(()=>{
            SceneLoadManager.Instance.GoToStart();
            if(AudioManager.Instance!=null) AudioManager.Instance.PlayRandomSound("Click");
        });
        restartButton.onClick.AddListener(() =>
        {
            SceneLoadManager.Instance.Reload();
            if(AudioManager.Instance!=null) AudioManager.Instance.PlayRandomSound("Click");
        });
        
        var playerNames = "";
        int maxScore = 0;
        foreach (var playerName in SnakeManager.Instance.GetBigger(out maxScore))
        {
            playerNames += playerName + " ";
        }
        winText.text = $"{playerNames}的分数最高，\n为{maxScore}";
        gameOverPanel.SetActive(true);
    }
    #endregion

    //创建计时器
    public void CreateCountTimer(Transform parent, string title, float time, UnityEvent onEnd)
    {
        var timer = Instantiate(countTextPrefab, parent).GetComponent<CountText>();
        timer.Init(title, time, onEnd);
    }

    #region 冲刺
    private void Rush()
    {
        SnakeManager.Instance.AllSpeedUp(deltaSpeed);
        ToolManager.Instance.UpdateGenerateInfoIndex();
        StartCoroutine(ShowPlayerMarkUI());
    }

    private IEnumerator ShowPlayerMarkUI()
    {
        UITip.SetActive(true);
        yield return new WaitForSeconds(1);
        UITip.SetActive(false);
    }
    #endregion
}
