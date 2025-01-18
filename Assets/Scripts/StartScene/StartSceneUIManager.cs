using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartSceneUIManager : MonoBehaviour
{
    public Button startButton;
    public Button teamButton;
    public Button exitButton;

    public GameObject startPanel;
    public GameObject teamPanel;

    public Text startPanelText;
    public Button singleButton;
    public Button doubleButton;
    public Button tripleButton;
    public Button quadrupleButton;
    public Button closeStartPanelButton;
    
    public Button closeTeamPanelButton;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startPanel.SetActive(false);
        teamPanel.SetActive(false);
        
        startButton.onClick.AddListener(StartButtonClicked);
        teamButton.onClick.AddListener(TeamButtonClicked);
        exitButton.onClick.AddListener(ExitButtonClicked);
        singleButton.onClick.AddListener(SingleButtonClicked);
        doubleButton.onClick.AddListener(DoubleButtonClicked);
        tripleButton.onClick.AddListener(TripleButtonClicked);
        quadrupleButton.onClick.AddListener(QuadrupleButtonClicked);
        closeStartPanelButton.onClick.AddListener(CloseStartPanelButtonClicked);
        closeTeamPanelButton.onClick.AddListener(CloseTeamPanelButtonClicked);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateVolume();
            AudioManager.Instance.PlayMusic("Start");
        }
        Time.timeScale = 1;


    }

    private void StartButtonClicked()
    {
        startPanel.SetActive(true);
    }
    private void TeamButtonClicked()
    {
        teamPanel.SetActive(true);
    }
    private void ExitButtonClicked()
    {
        //停止游戏
        SceneLoadManager.Instance.OnExitGame();
    }

    private void SingleButtonClicked()
    {
        SceneLoad(1);
    }
    private void DoubleButtonClicked()
    {
        SceneLoad(2);
    }

    private void TripleButtonClicked()
    {
        SceneLoad(3);
    }

    private void QuadrupleButtonClicked()
    {
        SceneLoad(4);
    }

    private void CloseStartPanelButtonClicked()
    {
        startPanel.SetActive(false);
    }

    private void CloseTeamPanelButtonClicked()
    {
        teamPanel.SetActive(false);
    }
    
    

    private void SceneLoad(int playerNum)
    {
        SceneLoadManager.Instance.LoadScene("SampleScene");
    }

}
