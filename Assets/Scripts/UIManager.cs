using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;

    public GameObject gameWonUI;
    public GameObject gameLostUI;
    public GameObject gamePlayingUI;

    public enum UIState
    {
        GamePlaying,
        GameLost,
        GameWon
    }

    UIState currentUIState;

    private Dictionary<UIState, Tuple<GameObject, Action>> UIDict;


    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
       

        currentUIState = UIState.GamePlaying;

        UIDict = new Dictionary<UIState, Tuple<GameObject, Action>>();
        UIDict.Add(UIState.GamePlaying, new Tuple<GameObject, Action>(gamePlayingUI, ShowGamePlayingUI));
        UIDict.Add(UIState.GameLost, new Tuple<GameObject, Action>(gameLostUI, ShowGameOverUI));
        UIDict.Add(UIState.GameWon, new Tuple<GameObject, Action>(gameWonUI, ShowGameWinUI));

        gameManager.OnGameStateChangeAction += OnChangeUIGameStateChange;

        ShowUI(currentUIState);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentUIState == UIState.GameLost || currentUIState == UIState.GameWon)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameManager.RePlayGame();
            }
        }
    }

    void OnChangeUIGameStateChange(GameManager.GameState e)
    {
        if (e == GameManager.GameState.PlayerLost)
        {
            currentUIState = UIState.GameLost;
        } else if (e == GameManager.GameState.PlayerWon)
        {
            currentUIState = UIState.GameWon;
        }
        else
        {
            currentUIState = UIState.GamePlaying;
        }

        ShowUI(currentUIState);
        UIDict[currentUIState].Item1.SetActive(true);
    }

    private void ShowUI(UIState uIState) {
        foreach (KeyValuePair<UIState, Tuple<GameObject, Action>> pair in UIDict)
        {
            pair.Value.Item1.SetActive(pair.Key == uIState);
            
        }

        UIDict[uIState].Item2.Invoke();

    }

    private void ShowGamePlayingUI()
    {
        
    }

    void ShowGameOverUI()
    {

    }

    void ShowGameWinUI()
    {

    }

    private void OnDestroy()
    {
        gameManager.OnGameStateChangeAction -= OnChangeUIGameStateChange;

    }
}
