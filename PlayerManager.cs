using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager player_Manager = null;
    private MeshRenderer mesh_Renderer;
    private GameManager.MaterialState player_material_state;
    private Rigidbody rb_Body;
    [SerializeField]
    private Transform left_position, right_position;
    [SerializeField]
    private float ball_Speed;
    private bool is_invinsible;
    private Material default_material;
    private Color default_color;
    [SerializeField]
    private Material invinsible_material;
    private Vector3 default_speed;
    [SerializeField]
    private TrailRenderer trail;
    public static PlayerManager player_Manager_Instance
    {
        get
        {
            if (player_Manager == null)
            {
                player_Manager = new GameObject("Player").AddComponent<PlayerManager>();
            }
            return player_Manager;
        }
    }
    private void Awake()
    {
        mesh_Renderer = gameObject.GetComponent<MeshRenderer>();
        rb_Body = gameObject.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        DOTween.Init();
        is_invinsible = false;
    }
    private void Update()
    {
        GetInput();
        if(Input.GetKeyDown(KeyCode.I)){
            BecomeInvinsible();
        }
    }
    private void OnEnable()
    {
        player_Manager = this;
    }
    public void GetReadyPlayer()
    {
        SetPlayerMatState();
        SetPlayerColor();
    }
    private void SetPlayerMatState()
    {
        int random_num = Random.Range(1, 3);
        if (random_num == 1)
        {
            player_material_state = GameManager.MaterialState.Material1;
        }
        else
        {
            player_material_state = GameManager.MaterialState.Material2;
        }
    }
    private void SetPlayerColor()
    {
        if (player_material_state == GameManager.MaterialState.Material1)
        {
            mesh_Renderer.material = GameManager.gameManager_Instance.mat1;
        }
        else
        {
            mesh_Renderer.material = GameManager.gameManager_Instance.mat2;
        }
        default_material = mesh_Renderer.material;
        default_color = mesh_Renderer.material.color;
    }
    public void StartGame()
    {
        rb_Body.velocity = Vector3.down * ball_Speed;
    }
    private void FinishGame(bool player_win)
    {
        if (player_win)
        {
            GameManager.gameManager_Instance.SetGameState(GameManager.GameStates.WIN);
        }
        else
        {
            GameManager.gameManager_Instance.SetGameState(GameManager.GameStates.DEATH);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            if (mesh_Renderer.material.ToString() == other.gameObject.GetComponent<MeshRenderer>().material.ToString())
            {
                LevelManager.level_Manager_Instance.DestroyFloor(other.gameObject.GetComponent<Cube>().GetCubePosition());
                LevelManager.level_Manager_Instance.GenerateNewFloor();
                GameObject.Destroy(other.gameObject);
            }
            else
            {
                if(!is_invinsible){
                    GameManager.gameManager_Instance.SetGameState(GameManager.GameStates.DEATH);
                }
                else{
                    LevelManager.level_Manager_Instance.DestroyFloor(other.gameObject.GetComponent<Cube>().GetCubePosition());
                    LevelManager.level_Manager_Instance.GenerateNewFloor();
                    GameObject.Destroy(other.gameObject);
                }
            }
        }
        if(other.gameObject.CompareTag("Invinsible")){
            if(!IsInvinsible()){
                BecomeInvinsible();
            }
            GameObject.Destroy(other.gameObject);
        }
        
    }
    private void GetInput()
    {
        if (GameManager.gameManager_Instance.GetGameState() == GameManager.GameStates.GAMEPLAY)
        {
            if (Input.GetMouseButton(0))
            {
                float i = Input.GetAxis("Mouse X");
                if (i >= 0.5f)
                {
                    GoRight();
                }
                else if (i < -0.5f)
                {
                    GoLeft();
                }
            }
        }
    }
    public void GoLeft()
    {
        gameObject.transform.DOMoveX(left_position.position.x, 0.1f);
    }
    public void GoRight()
    {
        gameObject.transform.DOMoveX(right_position.position.x, 0.1f);
    }
    public Vector3 GetPlayerSpeed()
    {
        return rb_Body.velocity;
    }
    public void FasterSpeed()
    {
        ball_Speed += 0.4f;
        rb_Body.velocity = Vector3.down * ball_Speed;
    }
    private void BecomeInvinsible(){
        is_invinsible = true;
        default_color = mesh_Renderer.material.color;
        mesh_Renderer.material = invinsible_material;
        trail.material = invinsible_material;
        ColorInvincible();
        PlayInvincibleAnim();
    }
    private void ColorInvincible(){
        if(is_invinsible){
            mesh_Renderer.material.DOColor(Random.ColorHSV(),0.2f).OnComplete(()=> ColorInvincible());
        }
        else{
            mesh_Renderer.material = default_material;
            mesh_Renderer.material.DOColor(default_material.color,0.2f);
        }
    }
    private void PlayInvincibleAnim(){
        Transform camera_transform = GameManager.gameManager_Instance.GetCameraTransform();
        default_speed = rb_Body.velocity;
        rb_Body.velocity = Vector3.zero;
        gameObject.transform.DOMoveY(gameObject.transform.position.y + 4,3).OnComplete(() => StartCoroutine(DiveInDelay()));
        camera_transform.DOMoveZ(camera_transform.position.z + 4,3.4f);
    }
    public void DiveIn(){
        rb_Body.velocity = Vector3.down * 60;
        StartCoroutine(InvisibleTime());
    }
    IEnumerator DiveInDelay(){
        yield return new WaitForSeconds(0.4f);
        DiveIn();
    }
    IEnumerator InvisibleTime(){
        yield return new WaitForSeconds(1.8f);
        if(default_speed.y > -40){
            rb_Body.velocity = default_speed;
        }
        else{
            rb_Body.velocity = 2 * default_speed / 3;
        }
        StartCoroutine(InvinsibleOverTime());
        default_speed = rb_Body.velocity;
        Transform camera_transform = GameManager.gameManager_Instance.GetCameraTransform();
        camera_transform.DOMoveZ(camera_transform.position.z -4,0.4f);
        camera_transform.DOMoveY(camera_transform.position.y + 1,0.2f);
    }
    IEnumerator InvinsibleOverTime(){
        yield return new WaitForSeconds(1.3f);
        is_invinsible = false;
    }
    public bool IsInvinsible(){
        return is_invinsible;
    }
}
