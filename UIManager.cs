using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager ui_Manager = null;
    private GameObject current_ui;
    [SerializeField]
    private GameObject idle_ui, gameplay_ui, win_ui,death_ui;
    [SerializeField]
    private TMP_Text score_text;
    public static UIManager ui_Manager_Instance
    {
        get
        {
            if (ui_Manager == null)
            {
                ui_Manager = new GameObject("UI_Manager").AddComponent<UIManager>();
            }
            return ui_Manager;
        }
    }
    private void OnEnable()
    {
        ui_Manager = this;
    }
    public void OpenUI(GameManager.GameStates new_state)
    {
        if (current_ui != null)
        {
            CloseUI();
        }
        switch (new_state)
        {
            case GameManager.GameStates.IDLE:
                idle_ui.SetActive(true);
                current_ui = idle_ui;
                break;
            case GameManager.GameStates.GAMEPLAY:
                gameplay_ui.SetActive(true);
                current_ui = gameplay_ui;
                break;
            case GameManager.GameStates.WIN:
                win_ui.SetActive(true);
                current_ui = win_ui;
                break;
            case GameManager.GameStates.DEATH:
                death_ui.SetActive(true);
                current_ui = death_ui;
                break;
        }
    }
    private void CloseUI()
    {
        current_ui.SetActive(false);
    }
    public void StartGame()
    {
        GameManager.gameManager_Instance.SetGameState(GameManager.GameStates.GAMEPLAY);
        UpdateScore(0);
    }
    public void LeftMoveButton()
    {
        PlayerManager.player_Manager_Instance.GoLeft();
    }
    public void RightMoveButton()
    {
        PlayerManager.player_Manager_Instance.GoRight();
    }
    public void UpdateScore(float score)
    {
        score_text.text = score.ToString();
    }
}
