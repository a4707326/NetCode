using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.UI;

public class StartCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform _canvas;
    public TMP_InputField _ip;
    public TMP_InputField PortIF;


    public Button createBtn;
    public Button joinBtn;


    //void Awake ()
    //{
    //    createBtn.onClick.AddListener(OnCreateBtn);
    //    joinBtn.onClick.AddListener(OnJoinBtn);

    //}

    void Start()
    {
        createBtn.onClick.AddListener(OnCreateBtn);
        joinBtn.onClick.AddListener(OnJoinBtn);
        
    }





    private void OnJoinBtn()
    {
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(_ip.text, ushort.Parse(PortIF.text));
        UnityTransport transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
        transport.SetConnectionData(endpoint);
        //transport.SetConnectionData(_ip.text, ushort.Parse(PortIF.text));
        NetworkManager.Singleton.StartClient();
    }
    private void OnCreateBtn()
    {
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(_ip.text, ushort.Parse(PortIF.text));

       var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
        transport.SetConnectionData(endpoint);
        //transport.SetConnectionData(_ip.text, ushort.Parse(PortIF.text));
        NetworkManager.Singleton.StartHost();
        GameManager.Instance.LoadScene("Lobby");

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
