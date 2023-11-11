using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    public Dictionary<ulong, PlayerInfo> AllPlayerInfos { get; private set; }

    public UnityEvent OnStartGame;


    private void Awake()
    {

    }
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.SceneManager.OnLoadEventCompleted += OnLoadEventComplete;

    }

    private void OnLoadEventComplete(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (sceneName == "Game")
        {
            OnStartGame.Invoke();
        }
    }


    public void StartGame(Dictionary<ulong,PlayerInfo> playerinfos)
    {
        AllPlayerInfos = playerinfos;
        UpdateAllPlayerInfos();
    }

    public void LoadScene(string sceneName)
    {
        NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


    void UpdateAllPlayerInfos()
    {
        foreach (var item in AllPlayerInfos)
        {
            UpdatePlayerInfoClientRpc(item.Value);//這裡會調用客戶端ClientRpc
        }
    }

    [ClientRpc]//客戶端
    void UpdatePlayerInfoClientRpc(PlayerInfo playerInfo)
    {
        if (AllPlayerInfos == null)
        {
            AllPlayerInfos = new();
        }
        if (!IsServer)
        {
            if (AllPlayerInfos.ContainsKey(playerInfo.id))
            {
                AllPlayerInfos[playerInfo.id] = playerInfo;
            }
            else
            {
                AllPlayerInfos.Add(playerInfo.id, playerInfo);
            }

        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
