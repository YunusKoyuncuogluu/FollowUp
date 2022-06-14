using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    [Serializable]
    public class UIPanels
    {
        public string ElemantName;
        public GameObject PanelObject;
        public Panels PanelID;
    }

    public enum Panels
    {
        None,
        MainMenu,
        GameFail,
        GameSuccess,
    }

    public List<UIPanels> uIPanels = new List<UIPanels>();

    void Start()
    {
        OpenPanel(Panels.MainMenu);
    }

    internal protected void OpenPanel(Panels panels)
    {
        AllPanelClose();

        foreach (UIPanels item in uIPanels)
        {
            if (panels == item.PanelID)
            {
                item.PanelObject.SetActive(true);
            }
        }
    }

    internal protected void AllPanelClose()
    {
        foreach (UIPanels item in uIPanels)
        {
            item.PanelObject.SetActive(false);
        }
    }

    public void SetPlayButton()
    {
        AllPanelClose();
        GameManager.Instance.SetGameState(GameManager.GameState.Playıng);
        GameManager.Instance.playerManager.Jump();
    }

    public void SetGameFail()
    {
        GameManager.Instance.DefaultValues();
        GameManager.Instance.playerManager.SetPlayerState(PlayerManager.PlayerState.Idle);
        OpenPanel(Panels.MainMenu);
    }

    public void SetGameSuccess()
    {
        GameManager.Instance.DefaultValues();
        GameManager.Instance.playerManager.SetPlayerState(PlayerManager.PlayerState.Idle);
        OpenPanel(Panels.MainMenu);
    }
}
