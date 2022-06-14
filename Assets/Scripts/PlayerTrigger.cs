using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private PlayerManager PlayerManager;
    internal bool Onetime = false;

    void Start()
    {
        PlayerManager = transform.root.GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JumpingAreaController>() && !PlayerManager.OneTriggerJump)
        {
            if (other.GetComponent<JumpingAreaController>().IsFactor)
            {
                other.GetComponent<JumpingAreaController>().TextCountObject.SetActive(false);
                StartCoroutine(PlayerManager.AddTrail(other.GetComponent<JumpingAreaController>().CountFactor));
            }
            PlayerManager.Jump();
        }

        if (other.GetComponent<Obstacle>())
        {
            other.GetComponent<Obstacle>().UpdateObstacleFactor();
            PlayerManager.RemoveTrail(other.GetComponent<Obstacle>().IsSuccess);
        }

        if (other.CompareTag("Ground") && !Onetime)
        {
            Onetime = true;
            PlayerManager.SetPlayerState(PlayerManager.PlayerState.Move);
        }

        if (other.CompareTag("Fail"))
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.Fail)
                GameManager.Instance.GameFail();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerManager.OneTriggerJump = false;
    }
}
