using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameCtrl : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform _canvas;

    [SerializeField]
    CinemachineVirtualCamera _cameraCtrl;

    TMP_InputField _inputIF;
    RectTransform _contentRTF;
    GameObject _dialogCell;
    Button _sendBtn;


    public static GameCtrl Instance;


    void Awake()
    {
        Instance = this;
        _inputIF = _canvas.Find("Dialog/InputIF").GetComponent<TMP_InputField>();
        _contentRTF = _canvas.Find("Dialog/DialogPanelSV/Viewport/Content") as RectTransform;
        _dialogCell = _contentRTF.Find("Cell").gameObject;
        _sendBtn = _canvas.Find("Dialog/SendBtn").GetComponent<Button>();
        _sendBtn.onClick.AddListener(OnSendBtnClick);
    }

    public override void OnNetworkDespawn()
    {
        //_inputIF = _canvas.Find("Dialog/InputIF").GetComponent<TMP_InputField>();
        //_contentRTF = _canvas.Find("Dialog/DialogPanelSV/Viewport/Content") as RectTransform;
        //_dialogCell = _contentRTF.Find("Cell").gameObject;
        //_sendBtn = _canvas.Find("Dialog/SendBtn").GetComponent<Button>();
        //_sendBtn.onClick.AddListener(OnSendBtnClick);
        base.OnNetworkDespawn();
    }



    private void OnSendBtnClick()
    {
        if (string.IsNullOrEmpty(_inputIF.text)) 
        {
            return;
        }

        PlayerInfo playerInfo = GameManager.Instance.AllPlayerInfos[NetworkManager.Singleton.LocalClientId];
        AddDialogCell(playerInfo.name,_inputIF.text);

        if (IsServer)
        {
            SendMegToOthersClientRpc(playerInfo, _inputIF.text);
        }
        else 
        {
            SendMegToOthersServerRpc(playerInfo, _inputIF.text);

        }

    }
    [ClientRpc]
    void SendMegToOthersClientRpc(PlayerInfo playerInfo,string content)
    {
        if(!IsServer && NetworkManager.LocalClientId != playerInfo.id) 
        {
            AddDialogCell(playerInfo.name, content);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void SendMegToOthersServerRpc(PlayerInfo playerInfo, string content)
    {
        AddDialogCell(playerInfo.name, content);
        SendMegToOthersClientRpc(playerInfo, content);
    }



    void AddDialogCell(string playerName, string content)
    {
        GameObject clone = Instantiate(_dialogCell);
        clone.transform.SetParent(_contentRTF, false);
        clone.AddComponent<DialogCell>().Initial(playerName, content);
        clone.SetActive(true);
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetFollowTarget(Transform target)
    {
        _cameraCtrl.Follow = target;
    }

    public Vector3 GetSpawnPos()
    {
        Vector3 pos = new Vector3();
        Vector3 offest = transform.forward * Random.Range(-10f, 10f) + transform.right * Random.Range(-10f, 10f);
        pos = transform.position + offest;
        
        return pos;
    }
}
