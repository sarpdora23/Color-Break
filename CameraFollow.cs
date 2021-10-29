using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    private Rigidbody rb_body;
    private bool can_follow = false;
    private bool one_time = true;
    private void Awake()
    {
        rb_body = gameObject.GetComponent<Rigidbody>();
    }
    private void Start() {
        DOTween.Init();
    }
    private void Update()
    {
        if (GameManager.gameManager_Instance.GetGameState() == GameManager.GameStates.GAMEPLAY)
        {
            if (one_time)
            {
                one_time = false;
                StartCoroutine(FollowDelay1());
            }
            if (can_follow)
            {
                rb_body.velocity = new Vector3(0,PlayerManager.player_Manager_Instance.GetPlayerSpeed().y,0);
            }
        }
    }
    IEnumerator FollowDelay1()
    {
        yield return new WaitForSeconds(0.8f);
        rb_body.velocity = new Vector3(0,PlayerManager.player_Manager_Instance.GetPlayerSpeed().y * 2 /3,0);
        StartCoroutine(FollowDelay2());
        gameObject.transform.DOMoveZ(gameObject.transform.position.z - 8, 4);
    }
    IEnumerator FollowDelay2(){
        yield return new WaitForSeconds(0.4f);
        can_follow = true;
    }


}
