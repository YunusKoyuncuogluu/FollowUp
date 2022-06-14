using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static readonly object Pad = new object();

    internal protected enum GameState
    {
        None,
        Idle,
        Playıng,
        Pause,
        Win,
        Fail,
    }

    [SerializeField] internal protected GameState CurrentGameState;
    [SerializeField] internal protected PlayerManager playerManager;
    [SerializeField] internal protected UIManager uIManager;
    [SerializeField] private FollowTarget followTargetCam;
    [SerializeField] internal protected StageController stageController;

    private void Awake()
    {
        lock (Pad)
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void GameSetUp()
    {

    }

    internal void DefaultValues()
    {
        playerManager.gameObject.SetActive(true);
        playerManager.DefaultValues();
        followTargetCam.Active = true;

        foreach (GameObject item in stageController.JumpPlatformList)
        {
            if (item.GetComponent<JumpingAreaController>())
                item.GetComponent<JumpingAreaController>().DefaultValue();
        }

        foreach (GameObject item in stageController.Obstacle)
        {
            item.SetActive(true);
            item.GetComponent<Obstacle>().Defaultvalue();
        }
    }

    internal protected void GameSuccess()
    {
        SetGameState(GameState.Win);
        followTargetCam.Active = false;
        uIManager.OpenPanel(UIManager.Panels.GameSuccess);
    }

    internal protected void GameFail()
    {
        SetGameState(GameState.Fail);
        followTargetCam.Active = false;
        uIManager.OpenPanel(UIManager.Panels.GameFail);
    }

    /// <summary>
    /// Oyun durumunu güncelleyen method
    /// </summary>
    /// <param name="gameState"></param>
    internal protected void SetGameState(GameState gameState)
    {
        CurrentGameState = gameState;
    }
}
