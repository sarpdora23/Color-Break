using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Rigidbody rb;
    private bool one_time = true;
    private bool follow = false;
    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void Update() {
        if(GameManager.gameManager_Instance.GetGameState() == GameManager.GameStates.GAMEPLAY){
            if(one_time){
                one_time = false;
                StartCoroutine(FollowDelay());
            }
        }
        if(follow){
            rb.velocity = PlayerManager.player_Manager_Instance.GetPlayerSpeed();
        }
    }
    IEnumerator FollowDelay(){
        yield return new WaitForSeconds(4);
        follow = true;
    }

}
