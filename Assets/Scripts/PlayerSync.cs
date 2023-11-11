using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSync : NetworkBehaviour
{
    NetworkVariable<Vector3> _syncPos = new NetworkVariable<Vector3>();
    NetworkVariable<Quaternion> _syncRot = new NetworkVariable<Quaternion>();
    Transform _syncTrasfrom;


    public void SetTarget(int gender)
    {
        _syncTrasfrom = transform.GetChild(gender);
    }


    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer) 
        {
            UploadTransform();
        }
    }
    private void FixedUpdate()
    {
        if (!IsLocalPlayer)
        {
            SyncTransform();
        }
    }

    private void SyncTransform()
    {
        Debug.Log( this.gameObject.name +"¤U¸ü" + _syncPos.Value);
        _syncTrasfrom.position = _syncPos.Value;
        _syncTrasfrom.rotation = _syncRot.Value;
    }

    private void UploadTransform()
    {
        Debug.Log(this.gameObject.name + "¤W¶Ç" +_syncTrasfrom.position);
        //Debug.Log(_syncTrasfrom.rotation);

        if (IsServer) 
        {
            _syncPos.Value = _syncTrasfrom.position;
            _syncRot.Value = _syncTrasfrom.rotation;
        }
        else
        {
            UploadTransformServerRpc(_syncTrasfrom.position, _syncTrasfrom.rotation);
        }
    }

    [ServerRpc]
    private void UploadTransformServerRpc(Vector3 position, Quaternion rotation)
    {
        _syncPos.Value = position;
        _syncRot.Value = rotation;
    }
}
