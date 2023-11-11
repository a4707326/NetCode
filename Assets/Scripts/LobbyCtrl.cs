using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;




public struct PlayerInfo: INetworkSerializable
{
    public ulong id;
    public string name;
    public bool isReady;
    public int gender;


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref name);
        serializer.SerializeValue(ref isReady);
        serializer.SerializeValue(ref gender);
    }
}

public class LobbyCtrl : NetworkBehaviour
{

    [SerializeField]
    Transform _canvas;
    public Transform content;
    //GameObject _originCell;
    public GameObject originCell;
    public Button startBtn;
    public Toggle readyTog;

    //List<PlayerListCell> cellList;
    Dictionary<ulong,PlayerListCell> cellDict;
    Dictionary<ulong, PlayerInfo> allPlayerInfos;
    TMP_InputField _nameIF;

    private void Awake()
    {
        //Debug.Log("ss");
        //allPlayerInfos = new Dictionary<ulong, PlayerInfo>();
        //startBtn.onClick.AddListener(OnStartBtn);
        //readyTog.onValueChanged.AddListener(OnReadyTog);
        //celldict = new ();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void AddPlayer(PlayerInfo playerInfo)
    {
        allPlayerInfos.Add(playerInfo.id,playerInfo);
        GameObject clone = Instantiate(originCell);
        clone.transform.SetParent(content, false);
        PlayerListCell cell = clone.GetComponent<PlayerListCell>();
        cellDict.Add(playerInfo.id,cell);
        cell.Initial(playerInfo);
        clone.SetActive(true);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer) 
        {
            NetworkManager.OnClientConnectedCallback += OnClientConn;
        }

        allPlayerInfos = new Dictionary<ulong, PlayerInfo>();
        startBtn.onClick.AddListener(OnStartBtn);
        readyTog.onValueChanged.AddListener(OnReadyTog);
        cellDict = new();
        _nameIF = _canvas.Find("NameIFTMP").GetComponent<TMP_InputField>();

        Toggle male = _canvas.Find("Gender/MaleTog").GetComponent<Toggle>();
        Toggle female = _canvas.Find("Gender/FemaleTog").GetComponent<Toggle>();
        male.onValueChanged.AddListener(OnMaleToggle);
        female.onValueChanged.AddListener(OnFemaleToggle);

        PlayerInfo playerInfo = new()
        {
            id = NetworkManager.LocalClientId,
            isReady = false,
            gender = 0
        };
        playerInfo.name = $"玩家{playerInfo.id}";
        _nameIF.name = playerInfo.name;
        _nameIF.onEndEdit.AddListener(OnEndEdit);


        AddPlayer(playerInfo);
    }

    private void OnEndEdit(string arg0)
    {
        if (string.IsNullOrEmpty(arg0))
        {
            return;
        }

        PlayerInfo playerInfo = allPlayerInfos[NetworkManager.LocalClientId];
        playerInfo.name = arg0;
        allPlayerInfos[NetworkManager.LocalClientId] = playerInfo;
        cellDict[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
        if (IsServer)
        {
            UpdateAllPlayerInfos();
        }
        else
        {
            UpdateAllPlayerInfoServerRpc(playerInfo);
        }
    }

    private void OnFemaleToggle(bool arg0)
    {
        if (arg0) 
        {
            PlayerInfo playerInfo = allPlayerInfos[NetworkManager.LocalClientId];
            playerInfo.gender = 1;
            allPlayerInfos[NetworkManager.LocalClientId] = playerInfo;
            cellDict[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
            if (IsServer)
            {
                UpdateAllPlayerInfos();
            }
            else
            {
                UpdateAllPlayerInfoServerRpc(playerInfo);
            }

            BodyCtrl.Instance.SwitchGender(1);
        }
    }

    private void OnMaleToggle(bool arg0)
    {
        if (arg0)
        {
            PlayerInfo playerInfo = allPlayerInfos[NetworkManager.LocalClientId];
            playerInfo.gender = 0;
            allPlayerInfos[NetworkManager.LocalClientId] = playerInfo;
            cellDict[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
            if (IsServer)
            {
                UpdateAllPlayerInfos();
            }
            else
            {
                UpdateAllPlayerInfoServerRpc(playerInfo);
            }
            BodyCtrl.Instance.SwitchGender(0);
        }
       
    }

    private void OnClientConn(ulong obj)
    {
        PlayerInfo playerInfo = new()
        {
            id = obj,
            name = "玩家" + obj,
            isReady = false
        };
        AddPlayer(playerInfo);
        UpdateAllPlayerInfos();

    }
    void UpdateAllPlayerInfos()
    {
        bool canGo = true;

        foreach (var item in allPlayerInfos)
        {
            if (!item.Value.isReady)
            {
                canGo = false;
            }
            UpdatePlayerInfoClientRpc(item.Value);//這裡會調用客戶端ClientRpc
        }

        startBtn.gameObject.SetActive(canGo);


    }
    //clinet，server，host() 
    [ClientRpc]//客戶端
    void UpdatePlayerInfoClientRpc(PlayerInfo playerInfo)
    {
        if (!IsServer) 
        {
            if (allPlayerInfos.ContainsKey(playerInfo.id))
            {
                allPlayerInfos[playerInfo.id] = playerInfo;
            }
            else
            {
                AddPlayer(playerInfo);
            }
            UpdatePlayerCells();
        }

    }

    private void UpdatePlayerCells()
    {
        foreach (var item in allPlayerInfos)
        {
            //cellDict[item.Key].SetReady(item.Value.isReady);
            cellDict[item.Key].UpdateInfo(item.Value);
        }
    }

    private void OnReadyTog(bool arg0)
    {
        cellDict[NetworkManager.LocalClientId].SetReady(arg0);
        UpdatePlayerInfo(NetworkManager.LocalClientId, arg0);

        if (IsServer)
        {
            UpdateAllPlayerInfos();
        }
        else
        {
            UpdateAllPlayerInfoServerRpc(allPlayerInfos[NetworkManager.LocalClientId]);//這裡會調用服務端ServerRpc
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void UpdateAllPlayerInfoServerRpc(PlayerInfo player)//服務端才會執行
    {
        allPlayerInfos[player.id] = player;
        cellDict[player.id].UpdateInfo(player);
        UpdateAllPlayerInfos();
    }

    void UpdatePlayerInfo(ulong id, bool isReady)
    {
        PlayerInfo info = allPlayerInfos[id];
        info.isReady = isReady;
        allPlayerInfos[id] = info;
    }


    private void OnStartBtn()
    {
        GameManager.Instance.StartGame(allPlayerInfos);
        GameManager.Instance.LoadScene("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
