using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerListCell : MonoBehaviour
{

    public TMP_Text nameTMP;
    public TMP_Text readyTMP;
    public TMP_Text genderTMP;
    public PlayerInfo PlayerInfo { get; private set; }

    public void Initial(PlayerInfo playerInfo)
    {
        nameTMP.text = playerInfo.name;
        readyTMP.text = playerInfo.isReady ? "�ǳ�" : "���ǳ�";
        genderTMP.text = playerInfo.gender == 0 ? "�k" : "�k";
    }
    public void UpdateInfo(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        nameTMP.text = playerInfo.name;
        SetReady(playerInfo.isReady);
        SetGender(playerInfo.gender);
        //readyTMP.text = playerInfo.isReady ? "�ǳ�" : "���ǳ�";
        //genderTMP.text = playerInfo.gender == 0 ? "�k" : "�k";

    }

    internal void SetReady(bool arg0)
    {
        readyTMP.text = arg0 ? "�ǳ�" : "���ǳ�";
    }
    internal void SetGender(int gender)
    {
        genderTMP.text = gender == 0 ? "�k" : "�k";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
