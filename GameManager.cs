using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager gameManager = null;
    private GameStates current_Game_State;
    public Material mat1, mat2,mat3;
    [SerializeField]
    private Transform camera_transform;
    public static GameManager gameManager_Instance
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return gameManager;
        }
    }
    private void Start()
    {
        RandomColorGenerate();
        LevelManager.level_Manager_Instance.SetLevelManager(mat1, mat2);
        SetGameState(GameStates.IDLE);
    }
    
    private void OnEnable()
    {
        gameManager = this;
    }
    public enum GameStates
    {
        IDLE,
        GAMEPLAY,
        WIN,
        DEATH
    }
    public enum MaterialState
    {
        Material1,
        Material2
    }
    public GameStates GetGameState()
    {
        return current_Game_State;
    }
    public void SetGameState(GameStates new_state)
    {
        current_Game_State = new_state;
        switch (current_Game_State)
        {
            case GameStates.IDLE:
                Idle();
                break;
            case GameStates.GAMEPLAY:
                GamePlay();
                break;
            case GameStates.WIN:
                Win();
                break;
            case GameStates.DEATH:
                Death();
                break;
        }
    }
    void Idle()
    {
        UIManager.ui_Manager_Instance.OpenUI(GameStates.IDLE);
        LevelManager.level_Manager_Instance.GenerateLevel();
        PlayerManager.player_Manager_Instance.GetReadyPlayer();
    }
    void GamePlay()
    {
        UIManager.ui_Manager_Instance.OpenUI(GameStates.GAMEPLAY);
        PlayerManager.player_Manager_Instance.StartGame();
    }
    void Win()
    {
        UIManager.ui_Manager_Instance.OpenUI(GameStates.WIN);
        SceneManager.LoadScene("SampleScene");
    }
    void Death()
    {
        UIManager.ui_Manager_Instance.OpenUI(GameStates.DEATH);
        SceneManager.LoadScene("SampleScene");
    }
    private void RandomColorGenerate()
    {
        mat1.color = Random.ColorHSV();
        mat2.color = Random.ColorHSV();
        mat3.color = Random.ColorHSV();
        Debug.Log(mat1.color.r + " " + mat2.color.r);
        Debug.Log(mat1.color.g + " " + mat2.color.g);
        if (mat1.color.r - mat2.color.r <= 0.2f || mat1.color.g - mat2.color.g <= 0.2f || mat1.color.b - mat2.color.b <0.2f)
        {
            mat2.color = Random.ColorHSV();
        }
    }
    public Transform GetCameraTransform(){
        return camera_transform;
    }
}
