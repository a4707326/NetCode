using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerInit : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        GameManager.Instance.OnStartGame.AddListener(OnStartGame);
        base.OnNetworkSpawn();
    }

    private void OnStartGame()
    {
        PlayerInfo playerInfo = GameManager.Instance.AllPlayerInfos[OwnerClientId];
        Transform body = transform.GetChild(playerInfo.gender);
        body.gameObject.SetActive(true);
        body.GetComponent<Rigidbody>().isKinematic = false;
        //Transform other = transform.GetChild(1 - playerInfo.gender);
        //other.gameObject.SetActive(false);
        PlayerSync playerSync = GetComponent<PlayerSync>();
        playerSync.SetTarget(playerInfo.gender);
        playerSync.enabled = true;

        if (IsLocalPlayer)
        {
            GameCtrl.Instance.SetFollowTarget(body);
            body.GetComponent<PlayerMove>().enabled = true;
            //gameObject.GetComponent<PlayerMove>().enabled = true;

        }
        transform.position =  GameCtrl.Instance.GetSpawnPos();
    }
}
